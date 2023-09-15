cd Code
rm -rf Site
mkdir Site
dotnet publish -o Site
cp -r ../.platform/ Site
cp -r ../CodeDeployScripts/ Site
cp ../ImageColourSwap.service Site
cp ../ImageColourSwap.conf Site
cd ..