# Flight Simulator App
The Flight Simulator App is a WPF application that allow us to display flight data on a dedicated simulator.
Our users are flight researchers or pilots who want to view data, sampled at a certain rate during any flight.
The flight data includes the steering mode, speed, altitude direction, etc., and are recorded into a text file, which can be loaded in our app.
The app will play the data like a movie from the beginning of the recording until the end, it will graphically display the plane in relation to the earth, the rudder position, and additional flight data in a number of different views, including a view designed to find anomalies in the data.

## Interface With FlightGear
The simulator can be downloaded from ```https://www.flightgear.org``` for all common operating systems.
FlightGear can open any input-output medium (for example socket tcp / udp, http, files, serial ports, etc.) and write or read flight data. data / protocol.
At this point, launch the application and add IP and Port to connect to the server and click "Connect". Next, you must select a file that has flight information (csv) and an anomaly detector algorithm (DLL file) to detect the anomalies. Once you upload the csv file the flight will start and you will see the slider moving according to the rows of the csv file.

## Requirements For Development
* WPF
* C#
* C++

## Running The Flight Simulator App
In the settings we enter into the simulator we can tell it to collect the data defined in playback_small.xml and save it for example in a csv file, transmit it via socket, etc.
The settings are entered in the command line or in the GUI screen when run in the settings tab.
The following setting for example the simulator can transmit the same data as a client via tcp socket to a local server
Listening to Port 5400:
```
--generic=socket,in,10,127.0.0.1,5400,tcp,playback_small
--fdm=null
```
This way the app can get flight data that takes place in real time in the simulator.

## Features Provided To Flight Investigators
+ Upload a CSV file in which the flight data is recorded.
+ The app will play in a dedicated control a movie showing the plane at any given moment from the beginning of the flight to its end at the sampled pace.
++ The projection is to instruct the simulator to place the aircraft exactly at its current position on Earth, and at its current altitude, direction, and attitude.
+ You can skip any time on the flight using the scroll bar control. The control can be moved forward or backward at any time, and the plane display for that time.
+ The position of the main rudders of the aircraft can be seen in the joystick display. The joystick and rudders change position according to flight time.
+ The playback rate can be controlled.
+ Along with the image of the aircraft and the rudder status, you can also see a number of different data such as flight altitude, flight speed, flight direction and pitch, roll and yaw indices.
+ You can select a specific data from all the data that appears in the file, and explore it.
In addition, an updated point graph of the data can be seen when the x-axis is the time and the y-axis is the given value.
+ You can choose to explore a specific data, you can see the most correlative data graph from the various flight data as found in the file, next to the previous graph. The graph updates over time. The linear regression line between them can also be seen.
+ It is possible to understand when an abnormality occurred in the measurement of the instruments or in the various data in order to identify possible malfunctions in the aircraft. Detection algorithms can be used. You can load the flight data, and choose an algorithm for detecting anomalies. The algorithm will detect at what moments in time an anomaly occurred and will mark this prominently, thus one can effectively jump into that moment in time and investigate it.
These algorithms will be implemented separately as plug-ins, that is, DLLs that are dynamically loaded in the application after uploading.

