import os
import random
import numpy as np
import tensorflow as tf
from datetime import datetime
from sklearn.model_selection import train_test_split
from tensorflow.keras.models import Model
from tensorflow.keras.layers import Input, Conv2D, MaxPooling2D, Flatten, Dense, Concatenate, Layer
from tensorflow.keras.preprocessing.image import load_img, img_to_array

# Function to extract date, satellite position and rotation from filename
def extract_data_from_filename(filename):
  # Extract the relevant parts of the filename
  name_parts = filename.split('_')

  date_str = name_parts[1]
  sat_pos = np.array(list(map(float,name_parts[2].split(','))))
  sat_rot = np.array(list(map(float,name_parts[3].replace('.jpg','').split(','))))

  # Convert date to a more usable format (e.g., timestamp or separate features)
  date_time = datetime.strptime(date_str, "%d-%m-%Y %H:%M:%S.%f")
  timestamp = date_time.timestamp()  # This gives you a float representing the time

  return timestamp, sat_pos, sat_rot

#filename = "cubesat_18-09-2024 21/21/57.240476_5878.28625250211,-1010.25371810028,3148.8815068145_0.9887074,-0.117828,-0.04034194,0.08334854.jpg"
#print(extract_data_from_filename(filename))


def load_image(image_path):
  img = load_img(image_path, target_size=(102, 102))
  img = img_to_array(img) / 255.0  # Normalize pixel values
  return img


class QuaternionNormalization(Layer):
  def call(self, inputs):
    # Normalize the quaternion to ensure the output has unit length
    return tf.nn.l2_normalize(inputs, axis=-1)

def create_model():
  # CNN for image processing
  image_input = Input(shape=(102, 102, 3))
  x = Conv2D(32, kernel_size=(3, 3), activation='relu')(image_input)
  x = MaxPooling2D(pool_size=(2, 2))(x)
  x = Conv2D(64, kernel_size=(3, 3), activation='relu')(x)
  x = MaxPooling2D(pool_size=(2, 2))(x)
  x = Flatten()(x)

  # Date and satellite position as additional inputs
  numerical_input = Input(shape=(1 + 3,))  # 1 for timestamp (or 1 for timestamp and additional features) + 3 for sat position

  # Combine image features and numerical inputs
  combined = Concatenate()([x, numerical_input])

  # Fully connected layers
  y = Dense(128, activation='relu')(combined)
  y = Dense(64, activation='relu')(y)

  # Output layer for satellite rotation prediction (4 values)
  output = Dense(4, activation='linear')(y)

  # Normalize quaternion output to ensure it has unit length
  quaternion_output = QuaternionNormalization()(output)

  # Define model
  model = Model(inputs=[image_input, numerical_input], outputs=quaternion_output)

  # Compile the model
  model.compile(optimizer='adam', loss='mse', metrics=['accuracy'])

  model.summary()
  
  return model
  
def generate_dataset():
  # Load images and corresponding data
  image_folder = './SyntheticImages'
  images = []
  numerical_data = []
  targets = []

  # Get all filenames in the folder
  all_filenames = os.listdir(image_folder)

  # Select 200 random filenames from the list
  selected_filenames = random.sample(all_filenames, 1000)

  for filename in selected_filenames:
    image_path = os.path.join(image_folder, filename)
    img = load_image(image_path)
    timestamp, sat_pos, sat_rot = extract_data_from_filename(filename)

    images.append(img)
    numerical_data.append(np.concatenate([[timestamp], sat_pos]))  # Concatenate timestamp and satellite position
    targets.append(sat_rot)  # Satellite rotation is the target, is a quaternion already normalized

  images = np.array(images)
  numerical_data = np.array(numerical_data)
  targets = np.array(targets)
  
  return images, numerical_data, targets
  
if __name__ == '__main__':
  print("Num GPUs Available: ", len(tf.config.experimental.list_physical_devices('GPU')))

  model = create_model()
  
  images, numerical_data, targets = generate_dataset()

  # Split data into training and validation sets
  X_train_img, X_val_img, X_train_num, X_val_num, y_train, y_val = train_test_split(
    images, numerical_data, targets, test_size=0.2, random_state=42
  )

  # Train the model
  history = model.fit(
    [X_train_img, X_train_num], y_train,
    validation_data=([X_val_img, X_val_num], y_val),
    epochs=100,
    batch_size=32
  ) 