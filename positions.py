from skyfield.api import load
from skyfield.vectorlib import VectorSum

# Load the ephemeris data
eph = load('de421.bsp')

# Define the time
ts = load.timescale()
time = ts.utc(2024, 7, 9, 12, 0, 0)

# Get the positions of the Earth and Sun
earth = eph['earth']
sun = eph['sun']

# Calculate the heliocentric position of the Earth
earth_position = earth.at(time).observe(sun).apparent()
earth_heliocentric = earth_position.position

# Calculate the heliocentric position of the Sun (which is approximately the center of the solar system)
sun_position = sun.at(time).observe(sun).apparent()
sun_heliocentric = sun_position.position

# Since the sun is at the center, its position is (0, 0, 0)
sun_heliocentric = [0, 0, 0]

print(f"Heliocentric position of Earth (in au): {earth_heliocentric}")
print(f"Heliocentric position of Sun (in au): {sun_heliocentric}")
