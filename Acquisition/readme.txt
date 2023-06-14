$Id: readme.txt 36912 2021-02-16 08:14:04Z chm $
Copyright © 2012 Triamec Motion AG

Caution: you may harm your hardware when executing sample applications 
without adjusting configuration values to your hardware environment.
Please read and follow the recommendations below
before executing any sample application.


Overview
--------

The acquisition sample is a .NET Windows Forms application demonstrating how to
record data from your devices using the Triamec Automation and Motion software (TAM Software).

While the interesting code is concentrated in 8 methods of class AcquisitionForm,
you see a couple of other files and methods which contribute to creating a proper .NET windows forms application.


Hardware requirements
---------------------

- at least one servo-drive which allows to persist the axis name,
- a connection between the drive and the PC, be it via USB, Ethernet or a Tria-Link PCI adapter.
- a motor and encoder connected to the servo-drive,
- power supply for the servo-drive logic and motor power.


Software environment adjustment
-------------------------------

MSChart is used in order to visualize the recorded data. Before you can compile
this sample, please invoke the installer in the ext\MSChart directory, or
visit the official download page https://www.microsoft.com/en-us/download/details.aspx?id=14422.


Hardware configuration adjustment
---------------------------------

The sample assumes a configured axis which can be moved freely between -1 and 1. The axis name must be 'X'. The duration
of axis operations is limited to 10 seconds.
All these requirements can be configured in the code by means of constants.
Specifically, you've to set the _moveAxis field to true in order to have the axis controlled by this application.


What the program does
---------------------

As soon as the AcquisitionForm is shown,
- a TAM topology is created,
- the Tria-Link gets booted,
- the servo-drive gets searched.

After this, the servo-drive is immediately enabled, and an endless move
forth-and-back sequence is started, while simultaneously recording data and
showing it in the chart. Data is recorded at the highest possible rate triggered
by the raising edge of the current velocity of the axis.

The user can
- change the level of the trigger.
- change the recording duration.

The program also demonstrates how the acquisitions framework allows to record
data while holding the UI responsive.


Shortcomings
------------

MSChart is deprecated.

You may notice that the movements are sometimes delayed. This behavior is
inherent to .NET and the Windows operating system. You may be able to reduce
peak delays by elevating process and/or thread priorities and making use of
GC.Collect, but you won't get any guarantees about the maximum delay.
If you have a situation where code needs to be executed in some limited period
of time, consider to write a custom firmware extension by means of a Tama
program (see the Tama Library sample), or employ a real time system like
TwinCAT.