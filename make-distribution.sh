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
tag=13981
distdir=diva-r$tag

# Create distribution directory and start filling it
cd $wd
echo Making Diva Distribution $distdir
mkdir $distdir
mkdir $distdir/doc
cp -r $opensimdir/bin $distdir
cp -r $opensimdir/WifiPages $distdir
cp $opensimdir/README.txt $distdir/OSREADME.txt
cp $opensimdir/CONTRIBUTORS.txt $distdir/OSCONTRIBUTORS.txt
cp $opensimdir/LICENSE.txt $distdir/OSLICENSE.txt
cp -r $opensimdir/ThirdPartyLicenses $distdir

# Clean up
echo Cleaning up
chmod +rwx $distdir -R
rm $distdir/WifiPages/*~
cd $distdir/bin
rm Robust* SimpleApp*
rm OpenSim.TestSuite* Prebuild.exe* OpenSim.Tests.Clients*
rm SubversionSharp.* svn_client-1.dll 
rm OpenSim.Grid.*

# Unsed DBs
rm libdb_dotNET43.dll libdb44d.dll System.Data.SQLite.dll OpenSim.Data.SQLiteLegacy.dll OpenSim.Data.MSSQL.dll* OpenSim.Data.SQLite.dll*

# Unused Physics
rm libbulletnet.so libbulletnet.dll Modified.XnaDevRu.BulletX.dll Physics/OpenSim.Region.Physics.Bullet* Physics/OpenSim.Region.Physics.BulletXPlugin.* Physics/OpenSim.Region.Physics.Basic* Physics/OpenSim.Region.Physics.PhysX* Physics/OpenSim.Region.Physics.POS*

# Unused plugins
rm OpenSim.ApplicationPlugins.Rest.dll* OpenSim.ApplicationPlugins.Rest.Inventory* OpenSim.ApplicationPlugins.Rest.Regions.dll*

# Unused script engines
# Unfortunately there's some dependency...
#rm OpenSim.Region.ScriptEngine.Shared.YieldProlog.dll*

# Unused clients
rm *Sirikata* *VWoHTTP*

# Unused Diva components
rm Diva.Data.MySQL.* Diva.LoginService.* Diva.Wifi.ProcessorTest.*

# Misc
rm OpenSim.Tools.lslc.*

rm *.pdb *.log *.ini *.jpg *.JPG
rm -rf addin-db-* *.Tests.dll *.Tests.*.dll TestResult.* *.Tests.dll.xml config-include/storage
rm config-include/* j2kDecodeCache/* Regions/* DataSnapshot/* 

# Copy config, license and doc files
echo Copying config and doc files
cd $wd
cp Configs/README.txt $distdir
cp Configs/RELEASENOTES.txt $distdir
cp Configs/RELEASENOTESWIFI.txt $distdir
cp Configs/LICENSE.txt $distdir
cp Configs/LICENSEWIFI.txt $distdir
cp Configs/LICENSEIMAGES.txt $distdir
cp Configs/MI.txt $distdir
cp Configs/DotNetZip.txt $distdir/ThirdPartyLicenses

cp Configs/IMPORTANT.txt $distdir

cp Configs/OpenSim.ini $distdir/bin
cp Configs/DivaPreferences.ini $distdir/bin/config-include
cp Configs/MyWorld.ini.example $distdir/bin/config-include
cp Configs/RegionConfig.ini.example $distdir/bin/Regions

cp Configs/INSTALL.txt $distdir
cp Configs/doc/ADVANCED.txt $distdir/doc
cp Configs/doc/DNS.txt $distdir/doc
cp Configs/doc/MYSQL.txt $distdir/doc
cp Configs/doc/MONO.txt $distdir/doc
cp Configs/doc/OSQUESTIONS.txt $distdir/doc
cp Configs/doc/TROUBLESHOOTING.txt $distdir/doc
cp Configs/doc/WIFI.txt $distdir/doc

# Copy tools
echo Copying tools
cp $toolsdir/bin/Configure.exe $distdir/bin
cp $toolsdir/bin/Update.exe $distdir/bin
cp $toolsdir/bin/Ionic.Zip.dll $distdir/bin

# Copy Library
echo Copying library
cp $libdir/"Clothing Library (small).iar" $distdir/bin/Library
cp $libdir/"Objects Library (small).iar" $distdir/bin/Library

# Zip it
echo Zipping...
chmod +rwx $distdir -R
#/c/OptPrograms/cygwin/bin/zip -r $distdir.zip $distdir > out
zip -r $distdir.zip $distdir > out
rm out



