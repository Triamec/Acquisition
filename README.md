# Aquisition

[![TAM - API](https://img.shields.io/static/v1?label=TAM&message=API&color=b51839)](https://www.triamec.com/en/tam-api.html)

The acquisition sample is a .NET Windows Forms application demonstrating how to record data from your devices using the Triamec Advanced Motion (TAM Software).

The servo-drive is either recording continously (if no axis movement is occuring) or on a velocity trigger via an endless move forth-and-back sequence.
The recorded data is shown in a chart. By default, data is recorded at the highest possible rate.

Caution: you may harm your hardware when executing sample applications 
without adjusting configuration values to your hardware environment.
Please read and follow the recommendations below
before executing any sample application.

![TAM Aquisition](./doc/Screenshot.png)

## Hardware Prerequisites

- *Triamec* drive with a motor and encoder connected and configured with a stable position controller
- The sample assumes a configured axis which can be moved freely between -1 and 1
- Connection to the drive by *Tria-Link* (via PCI adapter), *USB* or *Ethernet*

## Software Prerequisites

This project is made and built with [Microsoft Visual Studio](https://visualstudio.microsoft.com/en/).

In addition you need [TAM Software](https://www.triamec.com/en/tam-software-support.html) installation.

## Run the *Aquisition* Application

1. Open the `Aquisition.sln`.
2. Open the `AquisitionForm.cs` (view code)
3. Set the name of the axis for `AxisName`. Double check it in the register *Axes[].Information.AxisName* using the *TAM System Explorer*.
4. Set the name of the network interface card for `NicName`. You can find this name using the *TAM System Explorer*. In the example below, `NicName = "Ethernet 2"`.
![TAM Aquisition](./doc/Screenshot.png)
5. Set `_moveAxis` to `true` if you want to use the trigger for the aquisition
6. Select the correct connection to the drive by using comment/uncomment for setting the `access` variable 

## Operate the *Hello World!* Application

- Use the slider **Trigger** to adjust the velocity needed to start the aquisition. If `_moveAxis` is set to `false`, **Trigger** is ignored (continous aquisition)
- Use the slider **Recording time** to adjust the length of the aquisition

