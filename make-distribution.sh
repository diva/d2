#! /usr/bin/sh

#opensimdir=/c/dev/divadistribution-git/opensim
opensimdir=c:/dev/divadistro/opensim
toolsdir=C:/dev/miservices-git/miservices/divadistribution/Tools
libdir=C:/dev/miservices-git/miservices/divadistribution/Library
wd=`pwd`

# Get the tag
cd $opensimdir
tag=`C:/Program\ Files/Git/bin/git show-ref --tags | tail -1`
tag=`echo ${tag:53}`
distdir=diva-r$tag

# Create distribution directory and start filling it
cd $wd
echo Making MetaverseInk Diva distribution $distdir
mkdir $distdir
cp -r $opensimdir/bin $distdir
cp $opensimdir/README.txt $distdir/OSREADME.txt
cp $opensimdir/CONTRIBUTORS.txt $distdir/OSCONTRIBUTORS.txt
cp $opensimdir/LICENSE.txt $distdir/OSLICENSE.txt
cp -r $opensimdir/ThirdPartyLicenses $distdir

# Clean up
echo Cleaning up
chmod +rwx $distdir -R
cd $distdir/bin
rm OpenSim.Grid.*
rm OpenSim.Server.exe* OpenSim.Services.exe*
rm OpenSim.TestSuite.exe* Prebuild.exe*
rm SubversionSharp.dll svn_client-1.dll 

# Unsed DBs
rm NHibernate.dll libdb_dotNET43.dll libdb44d.dll System.Data.SQLite.dll sqlite3.dll sqlite-3.4.1.so NHibernate.Mapping.Attributes.dll OpenSim.Data.MSSQL.dll* OpenSim.Data.NHibernate.dll* OpenSim.Data.SQLite.dll*

# Unused Physics
rm libbulletnet.so libbulletnet.dll Modified.XnaDevRu.BulletX.dll Physics/OpenSim.Region.Physics.BulletDotNETPlugin.dll Physics/OpenSim.Region.Physics.BulletXPlugin.dll 

# Unused plugins
rm OpenSim.ApplicationPlugins.Rest.dll* OpenSim.ApplicationPlugins.Rest.Inventory* OpenSim.ApplicationPlugins.Rest.Regions.dll*

# Unused script engines
# Unfortunately there's some dependency...
#rm OpenSim.Region.ScriptEngine.Shared.YieldProlog.dll*

# Misc
rm OpenSim.Tools.lslc.*

rm *.pdb *.log *.ini *.jpg *.JPG
rm -rf addin-db-* *.Tests.dll *.Tests.*.dll TestResult.*
rm config-include/* j2kDecodeCache/* Regions/* DataSnapshot/*

# Copy config files
echo Copying config files
cd $wd
cp Configs/INSTALL.txt $distdir
cp Configs/README.txt $distdir
cp Configs/LICENSE.txt $distdir
cp Configs/MYSQL.txt $distdir
cp Configs/MONO.txt $distdir
cp Configs/MI.txt $distdir
cp Configs/TROUBLESHOOTING.txt $distdir
cp Configs/RELEASENOTES.txt $distdir
cp Configs/OpenSim.ini $distdir/bin
cp Configs/DivaPreferences* $distdir/bin/config-include
cp Configs/MyWorld* $distdir/bin/config-include
cp Configs/RegionConfig* $distdir/bin/Regions
cp Configs/DotNetZip.txt $distdir/ThirdPartyLicenses

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
#/c/Opt/Cygwin/bin/zip -r $distdir.zip $distdir > out
zip -r $distdir.zip $distdir > out
rm out



