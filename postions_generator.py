import json
import ephem
from datetime import datetime, timedelta
from info_extractor import search_tle_by_date, sat_pos_and_vel, jday, sun_pos_from_sc

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

    return {
        'sun_pos': sun_pos,
        'sc_pos_i': pos,
        'jd': jd,
        'subsolar_point': subsolar_point,
    }

if __name__ == '__main__':
    positions = []
    N = 10
    starting_date = datetime.strptime('190724_083000', '%d%m%y_%H%M%S')
    # starting_date = datetime.utcnow()

    for i in range(0, N):
        new_date = starting_date + timedelta(minutes=i)
        positions.append(generate_position(new_date))

    with open('Simulation/Assets/Resources/generated_positions.json', 'w') as outfile:
        json.dump(positions, outfile, indent=2)
