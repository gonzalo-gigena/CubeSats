"""
Created by Elias Obreque
els.obrq@gmail.com
Date: 31-01-2023
SPEL UChile
"""

import os
from sgp4.api import Satrec
from sgp4.api import WGS84
import numpy as np
import json

re = 6378.137  # km
RAD2DEG = 180 / np.pi
DEG2RAD = 1 / RAD2DEG
au = 149597870.691  # km
rs = au

def jday(year, month, day, hour, minute, seconds):
    jd0 = 367.0 * year - 7.0 * (year + ((month + 9.0) // 12.0)) * 0.25 // 1.0 + 275.0 * month // 9.0 + day + 1721013.5
    utc = ((seconds / 60.0 + minute) / 60.0 + hour)  # utc in hours#
    return jd0 + utc / 24.

def search_tle_by_date(date_time):
    jd = jday(
        float('20' + date_time[4:6]),
        float(date_time[2:4]),
        float(date_time[:2]),
        float(date_time[7:9]) - 1.0,
        float(date_time[9:11]),
        float(date_time[11:])
    )
    jd_year = jday(float('20' + date_time[4:6]), 1, 1, 0, 0, 0)
    epoch_day = jd - jd_year

    current_epoch_tle = float(date_time[4:6] + str(round(epoch_day, 8)))

    file_tle = open("sat000052191.txt", 'r').read()
    epoch_day_tle = [float(line[17:33]) for line in file_tle.split('\n')[:-1] if line[0] == '1']

    idx = np.argmin(np.abs(current_epoch_tle - np.array(epoch_day_tle)))
    line_1 = file_tle.split('\n')[0 + 2 * idx]
    line_2 = file_tle.split('\n')[0 + 2 * idx + 1]

    return line_1, line_2, jd

def node_sat(image_name):
    node = 'NAN'
    sat = 'NAN'
    if 'N7' in image_name:
        node = 'N7'
    elif 'N4' in image_name:
        node = 'N4'
    if 'S3' in image_name:
        sat = 'S3'
    elif 'PS' in image_name:
        sat = 'PS'
    return node, sat

def get_file_info(image_name):
    date_time = image_name[:13]
    
    line1, line2, jd = search_tle_by_date(date_time)
    
    node, sat = node_sat(image_name)
    
    #High-speed computation of satellite positions and velocities
    satellite = Satrec.twoline2rv(line1, line2, WGS84)
    _, pos, vel = satellite.sgp4(int(jd), jd % 1)

    sun_pos_i_earth = calc_sun_pos_i(jd)
    sun_pos_from_sc = sun_pos_i_earth - pos
    
    return jd, sat, node, pos, vel, sun_pos_from_sc, line1, line2


def calc_sun_pos_i(jd):
    # all in degree
    n = jd - 2451545.0
    l = (280.459 + 0.98564736 * n) % 360.0
    m = (357.529 + 0.98560023 * n) % 360.0
    m *= DEG2RAD
    lam = (l + 1.915 * np.sin(m) + 0.0200 * np.sin(2 * m)) % 360.0
    lam *= DEG2RAD
    e = 23.439 - 3.56e-7 * n
    e *= DEG2RAD

    r_sun = (1.00014 - 0.01671 * np.cos(m) - 0.000140 * np.cos(2 * m)) * au
    u_v = np.array([np.cos(lam), np.cos(e) * np.sin(lam), np.sin(lam) * np.sin(e)])
    return r_sun * u_v

if __name__ == '__main__':
    images =  os.listdir('./images')

    dataset_info = []
    # loop through each image in the dataset
    for image in images:
        jd, sat, node, pos_i, vel_i, sun_pos_from_sc, line1, line2 = get_file_info(image)
        dataset_info.append({
            'sun_pos': sun_pos_from_sc.tolist(),
            'sc_pos_i': pos_i,
            'sc_vel_i': vel_i,
            'jd': jd,
            'filename': image,
            'line1': line1,
            'line2': line2,
            'sat_name': sat,
            'sat_node': node
        })

    with open('DATA.json', 'w') as outfile:
        json.dump(dataset_info, outfile, indent=2)
