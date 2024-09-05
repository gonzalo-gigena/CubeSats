import json
import ephem

from datetime import datetime, timedelta, UTC
from info_extractor import search_tle_by_date, sat_pos_and_vel, jday, sun_pos_from_sc

DATE_FORMAT = '%d-%m-%Y %H:%M:%S.%f'

def subsolar_point_at_utc(utc_datetime):
    # Initialize an observer location (use a central point on Earth)
    # Create an observer object at the center of the Earth
    observer = ephem.Observer()
    observer.lat, observer.lon = '0', '0'
    observer.date = utc_datetime
    
    # Create a Sun object and compute its position
    sun = ephem.Sun(observer)
    
    # Calculate the subsolar latitude
    subsolar_lat_deg = sun.dec * 180 / ephem.pi
    
    # Calculate the subsolar longitude
    gst = observer.sidereal_time() - sun.ra
    subsolar_lon_deg = gst * 180 / ephem.pi
    
    return subsolar_lat_deg, subsolar_lon_deg
     
def generate_position(dt):
    jd = jday(dt.year, dt.month, dt.day, dt.hour, dt.minute, dt.second)

    line1, line2= search_tle_by_date(jd, dt.year)
    
    pos, _ = sat_pos_and_vel(line1, line2, jd)

    sun_pos = sun_pos_from_sc(jd, pos)
    
    subsolar_point = subsolar_point_at_utc(dt)

    return sun_pos, subsolar_point, pos

if __name__ == '__main__':
    N = 100
    #starting_date = datetime.strptime('18-06-2024 00:00:00.000000', DATE_FORMAT)
    starting_date = datetime.now(UTC)
    
    positions = {
        'dates': [],
        'subsolar_points': [],
        'sun_pos': [],
        'satellites': [{
            'name': 'cubesat',
            'pos': []
        }]
    }

    for i in range(0, N):
        new_date = starting_date + timedelta(minutes=i)
        sun_pos, subsolar_point, sat_pos = generate_position(new_date)
        positions['dates'].append(new_date.strftime(DATE_FORMAT))
        positions['subsolar_points'].append(subsolar_point)
        positions['sun_pos'].append(sun_pos)
        positions['satellites'][0]['pos'].append(sat_pos)

    with open('Simulation/Assets/Resources/generated_positions.json', 'w') as outfile:
        json.dump(positions, outfile, indent=2)