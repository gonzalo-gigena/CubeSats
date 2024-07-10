# CubeSats

# Satellite Image Data Extraction

This script processes a set of satellite images to extract various information such as satellite positions, velocities, and sun positions. The extracted data is saved in a JSON file.

## Table of Contents

- [Requirements](#requirements)
- [Usage](#usage)
- [Functions](#functions)
  - [jday](#jday)
  - [search_tle_by_date](#search_tle_by_date)
  - [node_sat](#node_sat)
  - [get_file_info](#get_file_info)
  - [calc_sun_pos_i](#calc_sun_pos_i)
- [Author](#author)

## Requirements

- Python 3.x
- NumPy
- SGP4

## Usage

1. Place your satellite images in the `./images` directory.
2. Ensure you have the TLE data file named `sat000052191.txt` in the same directory as the script.
3. Run the script:

```sh
python info_extractor.py
```

4. The script will create a `DATA.json` file with the extracted information.

## Functions

### jday

Calculates the Julian Date from a given date and time.

### search_tle_by_date

Searches for the TLE (Two-Line Element) data closest to a given date and time.

### node_sat

Determines the node and satellite from the image filename.

### get_file_info

Extracts information from the image filename, including satellite position, velocity, and sun position.

### calc_sun_pos_i

Calculates the position of the sun in the inertial frame.