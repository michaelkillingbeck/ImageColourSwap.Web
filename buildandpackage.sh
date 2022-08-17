rm -rf Site
mkdir Site
dotnet publish -o Site
cp -r .platform/ Site
cp appspec.yml Site
cp -r Scripts/ Site
cp ImageColourSwap.service Site