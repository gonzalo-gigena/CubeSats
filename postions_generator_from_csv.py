import argparse
import json
import pandas as pd

from datetime import datetime
from postions_generator import subsolar_point_at_utc, DATE_FORMAT

def extract_name(column: str):
    name = column.split('_', 1)[0]
    return name

def extract_date_and_subsolar_point(dates_column: pd.Series):
    dates = dates_column.to_list()
    subsolar_points = dates_column.apply(
        lambda date: subsolar_point_at_utc(datetime.strptime(date, DATE_FORMAT))
    ).to_list()
    return dates, subsolar_points

def extract_satellite_data(row: pd.Series, satellite_name: str):
    x = float(row[satellite_name+'_sat_position_i(X)[m]'].replace(',', '.'))
    y = float(row[satellite_name+'_sat_position_i(Y)[m]'].replace(',', '.'))
    z = float(row[satellite_name+'_sat_position_i(Z)[m]'].replace(',', '.'))
    return[x, y, z]

def df_to_dict(df):
    positions = {
        'dates': [],
        'subsolar_points': [],
        'sun_pos': [],
        'satellites': []
    }
    
    dates, subsolar_points = extract_date_and_subsolar_point(df['Date time'])
    positions['dates'] = dates
    positions['subsolar_points'] = subsolar_points
    
    # 23 consecutive columns contain the data of a single satellite
    slice = 23
    for i in range(3, len(df.columns), slice):
        satellite = df.iloc[:, i:i+slice]
        
        satellite_name = extract_name(satellite.columns[0])
        satellite_pos = []
        for i in range(len(satellite)):
            data = extract_satellite_data(df.iloc[i], satellite_name)
            satellite_pos.append(data)

        positions['satellites'].append({
            'name': satellite_name,
            'pos': satellite_pos
        })
    return positions


def parse_command_line():
    # Create an argument parser object
    parser = argparse.ArgumentParser(description="Read and process a CSV file.")
    
    # Add an argument for the CSV file name
    parser.add_argument('csvfile', type=str, help='The path to the CSV file')
    
    # Parse the command-line arguments
    args = parser.parse_args()
    
    # Read the CSV file into a DataFrame
    df = pd.read_csv(args.csvfile)
    
    return df

if __name__ == '__main__':
    df = parse_command_line()
    positions = df_to_dict(df)
    
    with open('Simulation/Assets/Resources/generated_positions.json', 'w') as outfile:
        json.dump(positions, outfile, indent=2)
