You have downloaded Diva's binary distribution of the Wifi addon to
OpenSimulator. Please read LICENSEWIFI.txt before using this addon.

Wifi is a web front-end for OpenSimulator designed for small-to-medium
worlds. This distribution of Wifi targets grid configurations of
OpenSimulator. It is meant to run as a plugin to the Robust server that
you already have.

Note: Wifi is included in the standalone diva distribution of
OpenSimulator (http://github.com/diva/d2/downloads). If you are
running a non-diva standalone world and want to use Wifi, this package
will not be enough. I suggest you use the diva distribution instead,
because it already includes Wifi.

******************************************
***      INSTALLATION INSTRUCTIONS     ***
******************************************

TERMINOLOGY:

- WIFIDIR is the top directory of the Wifi package (this directory).
- ROBUSTDIR is the top opensim directory of your Robust server.

1 - Get your grid up & running. Make sure that you are running a
version of OpenSimulator that is compatible with this addon. The
OpenSimulator version for which this addon was built is stated in the
zip file name. If you have a different version of OpenSimulator, this
pre-built addon may or may not work. 

2 - Place this diva wifi package in the machine where you have your
Robust server running. The simulators have nothing to do with Wifi, so
you don't need to copy this package to those machines.

3 - Copy the entire WIFIDIR/WifiPages directory onto ROBUSTDIR. When
you're done, there should be a ROBUSTDIR/WifiPages directory, in
parallel with ROBUSTDIR/bin.

4 - Copy all files under WIFIDIR/bin onto ROBUSTDIR/bin.

5 - "Merge" WIFIDIR/bin/Wifi.ini.example onto your Robust server's ini
file. In other words, edit your Robust ini file and add the Wifi data
to it. More specifically: 

  a) Under the [Startup] section, add
  Diva.Wifi.dll:WifiServerConnector 
  to the long list of ServiceConnectors. You can add an explicit
  port to it, like the others have, so:
  8002/Diva.Wifi.dll:WifiServerConnector 

  b) Copy and paste the entire [WifiService] section to your Robust
  ini file and change the values of the configuration variables to fit
  your grid. See doc/WIFI.txt for *important* information about how to
  configure and use Wifi.

6 - Shutdown your Robust server and restart it.

7 - Point your Web browser to http://yourdomain:8002/wifi and take it
from there!


------------------------------------------

This distribution is available as-is; there are neither guarantees
nor support beyond the included documentation. 
For issue reports, please use
http://github.com/diva/diva-distribution/issues

*********************************************
*** For news, please follow me on Twitter ***
***    http://twitter.com/divacanto       ***
*********************************************

The diva distro does not maintain email lists or registrations.  If
you want to keep in touch, receive notifications of new releases,
etc., the best way is to follow me on Twitter, or visit my Twitter
page regularly.


