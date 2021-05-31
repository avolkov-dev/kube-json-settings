cd D:\Projects\Swisschain\kube-json-settings\SampleApp\SettingsSampleApp;
dotnet publish
cd D:\Projects\Swisschain\kube-json-settings\SampleApp\SettingsSampleApp\SettingsSampleApp\bin\Debug\net5.0
docker build  -t andreyvolkov/kube-sample-app .
docker login
docker push andreyvolkov/kube-sample-app