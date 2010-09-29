#! /usr/bin/sh

opensimdir=../opensim
toolsdir=Tools
libdir=Library
wd=`pwd`

# Get the tag
cd $opensimdir
#tag=`C:/Program\ Files/Git/bin/git show-ref --tags | tail -1`
#tag=`"C:/Program Files (x86)/Git/bin/git" show-ref --tags | tail -1`
#tag=`echo ${tag:53}`
distdir=wifi-0-7-0-2-b

# Create distribution directory and start filling it
cd $wd
echo Making Diva Wifi Distribution $distdir
mkdir $distdir
mkdir $distdir/bin
cp $opensimdir/bin/Diva.Wifi.dll $distdir/bin
cp $opensimdir/bin/Diva.Wifi.ScriptEngine.dll $distdir/bin
cp $opensimdir/bin/Diva.OpenSimServices.dll $distdir/bin
cp $opensimdir/addon-modules/Wifi/Wifi.ini.example $distdir/bin
cp -r $opensimdir/WifiPages $distdir
rm $distdir/WifiPages/*~

# Copy config, license and doc files
echo Copying config and doc files
cd $wd
mkdir $distdir/doc
cp Configs/READMEWIFI.txt $distdir
cp Configs/RELEASENOTESWIFI.txt $distdir
cp Configs/LICENSEWIFI.txt $distdir
cp Configs/LICENSEIMAGES.txt $distdir

cp Configs/doc/WIFI.txt $distdir/doc

# Zip it
echo Zipping...
chmod +rwx $distdir -R
#/c/OptPrograms/cygwin/bin/zip -r $distdir.zip $distdir > out
zip -r $distdir.zip $distdir > out
rm out



