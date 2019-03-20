<p  align="center"><img width="150" src=".github/Logo.png" alt="cover"></p>
<h1 align="center">
  Mobile Healthcare and Machine Learning
</h1>

<p  align="center">
<sup>
  <a href="https://github.com/mohyaboualam">Mohyeldin Aboualam</a>, 
  <a href="https://github.com/Scott-Bunting">Scott Bunting</a>, 
  <a href="https://github.com/fc2115">Felix Crowther</a>, 
  <a href="https://github.com/nebbles">Benedict Greenberg</a>, 
  <a href="https://github.com/josephine-latreille">Josephine Latreille</a>, 
  <a href="https://github.com/caoanle13">Cao An Le</a>, 
  <a href="https://github.com/leahpattison">Leah Pattison</a>
</sup>
</p>

<p  align="center">
<sup><sup>
  Department of Electrical and Electronic Engineering & Dyson School of Design Engineering, Imperial College London
</sup></sup>
</p>

<!-- <h4 align="center">
  <a href="#">More information coming soon...</a>
  <br><br>
 <img width="80" src="http://readthedocs.org/projects/de3-rob1-chess/badge/?version=latest" alt="Documentation Status"> 
</h4> -->

<!-- 
<p align="center">
	<sub>Design Engineering, Imperial College London</sub>
</p>
<br>
<p align="center">
	<a href="https://vimeo.com/291377091" >
	<img width="600" src="vimeo.png" alt="Click to play"></a>
</h1>
<br>
-->

**Sensa** is a mobile healthcare monitoring system which aims to use a blend of self reported measures and physiological signal analysis to detect stress and guide users to improve their state using personalised exercises.

### Code Structure

- The [hardware directory](https://github.com/nebbles/MHML/tree/develop/hardware) was used for firmware development of the M5Stack Fire, have a look at [the README](https://github.com/nebbles/MHML/tree/develop/hardware) for more info.
- The [app directory](https://github.com/nebbles/MHML/tree/develop/app) was used for the app development (using Unity) for iOS/Android, have a look at [the README](https://github.com/nebbles/MHML/tree/develop/app) for more info.
- The [server directory](https://github.com/nebbles/MHML/tree/develop/server) was used for backend server code such as the API (fronting the database) and machine learning elements, have a look at [the README](https://github.com/nebbles/MHML/tree/develop/server) for more info.

### Documentation

Throughout our code we have endeavored to leave useful comments and READMEs where suitable. In addition, to aid our cross-platform development within the team, we kept to project wide specifications.

- [The BLE Specification](https://github.com/nebbles/MHML/blob/develop/docs/BLE_Specification.md) defines agreed implementation of the Bluetooth Low Energy server (on the M5Stack).
- [The Data Specification](https://github.com/nebbles/MHML/blob/develop/docs/Data_Specification.md) defines agreed usage, type and structure of data throughout the system.  
- [The API Specification](https://github.com/nebbles/MHML/blob/develop/docs/API_Specification.md) defines agreed implementation and usage of the API (for accessing database and ML).

### Repository Structure Tree

*Note: This is a 'light' version of the project structure. For more information on files, refer to the relevant subdirectory README.*

```
MHML/
  ├─ .github/                     GitHub configuration files
  ├─ docs/API_Specification.md    Documentation on API usage
  ├─ docs/BLE_Specification.md    Documentation on BLE configuration and usage
  ├─ docs/Data_Specification.md   Documentation on cross platform data usage
  │
  ├─ app/                         Unity app development is stored
  │    ├─ Assets/Scripts/                    
  │    │  ├─ Main.cs              Main app logic
  │    │  ├─ Wifi/            
  │    │  │  └─ Networking.cs     Networking logic for communicating with API
  │    │  └─ Bluetooth/
  │    │     └─ controller.cs     BLE client logic for receiving from hardware
  │    └─ README.md               App specific readme
  │
  ├─ hardware/                    Firmware development for M5Stack Fire            
  │    ├─ lib/                    Custom and 3rd party libraries
  │    ├─ src/ble.h               BLE server logic
  │    ├─ src/data.h              Definition of central data structure
  │    ├─ src/gsr.h               GSR sensor development
  │    ├─ src/main.cpp            Main firmware runtime
  │    ├─ src/ppg.h               PPG sensor development
  │    └─ README.md               Hardware specific readme
  │
  ├─ server/
  │    ├─ api/                    Flask application for server API
  │    ├─ ml/                     Development of machine learning models
  │    └─ main.py     
  │
  ├─ LICENSE
  └─ README.md
```

### System Overview

<p align="center"><img width="700" src=".github/SystemOverview.png" alt="cover"></p>

### Project Development

From the early stages of the project, we anticipated how the division of development should operate in order to maximise our modularity and reduce interdependencies. This made integration at the later stages far more efficient. See below for a simple graphic of the process.

<p align="center"><img width="700" src=".github/ProcessMap.gif" alt="cover"></p>

### License

Our source code is licensed under [GNU General Public License v3.0](LICENSE)

<a rel="license" href="http://creativecommons.org/licenses/by-sa/4.0/"><img alt="Creative Commons License" style="border-width:0" src="https://i.creativecommons.org/l/by-sa/4.0/88x31.png" /></a><br />This work is licensed under a <a rel="license" href="http://creativecommons.org/licenses/by-sa/4.0/">Creative Commons Attribution-ShareAlike 4.0 International License</a>.
