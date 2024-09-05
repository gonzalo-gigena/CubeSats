from PIL import Image
import os

def compress():
    target_directory = './SyntheticImages'
    output_directory = './CompressedImages'

    for filename in os.listdir(target_directory):
        file_path = os.path.join(target_directory, filename)
        
        # Load the image
        image = Image.open(file_path)
        # Set the new size (e.g., 102x102 pixels)
        new_size = (102, 102)

        # Resize the image
        resized_image = image.resize(new_size)

        # Convert image to RGB if not already in that mode
        if resized_image.mode != 'RGB':
            resized_image = resized_image.convert('RGB')

        # Compress and save the resized image
        output_path = os.path.join(output_directory, filename)
        resized_image.save(output_path, quality=93)

        print(f'Resized and compressed {filename} saved.')

if __name__ == '__main__':
    compress()
