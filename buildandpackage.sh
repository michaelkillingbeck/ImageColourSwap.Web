rm -rf Site
mkdir Site
dotnet publish -o Site
cp -r .platform/ Site
cp appspec.yml Site