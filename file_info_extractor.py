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


def jday(year, mon, day, hr, minute, sec):
    jd0 = 367.0 * year - 7.0 * (year + ((mon + 9.0) // 12.0)) * 0.25 // 1.0 + 275.0 * mon // 9.0 + day + 1721013.5
    utc = ((sec / 60.0 + minute) / 60.0 + hr)  # utc in hours#
    return jd0 + utc / 24.


def get_file_info(name_image):
    dt = name_image[:13]
    # UTC + 1 ()
    jd = jday(float('20' + dt[4:6]), float(dt[2:4]), float(dt[:2]), float(dt[7:9]) - 1.0, float(dt[9:11]),
              float(dt[11:]))
    line1, line2 = search_tle_by_date(dt)
    node = None
    if 'N7' in name_image:
        node = 'N7'
    elif 'N4' in name_image:
        node = 'N4'
    else:
        node = 'NAN'
    sat = None
    if 'S3' in name_image:
        sat = 'S3'
    elif 'PS' in name_image:
        sat = 'PS'
    else:
        sat = 'NAN'
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


def search_tle_by_date(date_time):
    jd = jday(float('20' + date_time[4:6]), float(date_time[2:4]),
              float(date_time[:2]), float(date_time[7:9]) - 1.0,
              float(date_time[9:11]),
              float(date_time[11:]))
    jd_year = jday(float('20' + date_time[4:6]), 1, 1, 0, 0, 0)
    epoch_day = jd - jd_year
    current_epoch_tle = float(date_time[4:6] + str(round(epoch_day, 8)))
    file_tle = open("../sat000052191.txt", 'r').read()
    epoch_day_tle = [float(line[17:33]) for line in file_tle.split('\n')[:-1] if line[0] == '1']
    idx = np.argmin(np.abs(current_epoch_tle - np.array(epoch_day_tle)))
    line_1 = file_tle.split('\n')[0 + 2 * idx]
    line_2 = file_tle.split('\n')[0 + 2 * idx + 1]
    return line_1, line_2


if __name__ == '__main__':
    filename_list = []
    for filename in os.listdir():
        if '.jpg' in filename:
            filename_list.append(filename)

    dataset_info = {'sun_pos': [], 'sc_pos_i': [], 'sc_vel_i': [], 'jd': [], 'filename': [], 'line1': [], 'line2': [],
                    'sat_name': [], 'sat_node': []}
    # lop through each image in the dataset
    for earth_image in filename_list:
        jd, sat, node, pos_i, vel_i, sun_pos_from_sc, line1, line2 = get_file_info(earth_image)
        dataset_info['sun_pos'].append(sun_pos_from_sc)
        dataset_info['sc_pos_i'].append(pos_i)
        dataset_info['sc_vel_i'].append(vel_i)
        dataset_info['jd'].append(jd)
        dataset_info['filename'].append(earth_image)
        dataset_info['line1'].append(line1)
        dataset_info['line2'].append(line2)
        dataset_info['sat_name'].append(sat)
        dataset_info['sat_node'].append(node)

    with open("FILENAME.json", "w") as outfile:
        json.dump(dataset_info, outfile)
