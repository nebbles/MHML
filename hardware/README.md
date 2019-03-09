<h2 align="center"><br>Hardware Development</h2>
<br>

This directory contains the working development of the firmware for an M5Stack Fire integrated microcontroller device. The source code for this platform can be found [on GitHub](https://github.com/m5stack/M5Stack).



This directory uses [PlatformIO](https://platformio.org) (as [an extension](https://docs.platformio.org/en/latest/ide/vscode.html#quick-start) to VSCode). The `platformio.ini` file is configured to build straight to device.

Please note this directory is standalone to other directories. This means you must **open this directory alone, not the project level directory in VSCode**.

1. PlatformIO needs to have the `hardware/` directory set in the workspace. This is so it can "see" the `platformio.ini` file and `src/` directory, etc.
2. The `hardware/` directory cannot be active in the workspace alongside the `MHML/` project directory. This is due to [a known bug](https://github.com/Microsoft/vscode-cpptools/issues/1073#issuecomment-460797478) with vscode-cpptools, which causes files to be ruined when formatting C++ files.
