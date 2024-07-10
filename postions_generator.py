import json
from datetime import datetime, timedelta
from info_extractor import search_tle_by_date, sat_pos_and_vel, jday, sun_pos_from_sc


def generate_position(dt):
    jd = jday(dt.year, dt.month, dt.day, dt.hour - 1.0, dt.minute, dt.second)

    line1, line2= search_tle_by_date(jd, dt.year)
    
    pos, vel = sat_pos_and_vel(line1, line2, jd)

    sun_pos = sun_pos_from_sc(jd, pos)
    
    return {
        'sun_pos': sun_pos,
        'sc_pos_i': pos,
        'sc_vel_i': vel,
        'jd': jd,
        'line1': line1,
        'line2': line2,
    }

if __name__ == '__main__':
    positions = []
    N = 1
    starting_date = datetime.strptime('040822_154626', '%d%m%y_%H%M%S')
    
    for i in range(0, N):
        new_date = starting_date + timedelta(hours=i*2)
        positions.append(generate_position(new_date))

    with open('generated_positions.json', 'w') as outfile:
        json.dump(positions, outfile, indent=2)
