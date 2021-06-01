kubectl delete configmap kube-sample-app-appsettings  --ignore-not-found
# enumerate all needed files to config the application
# these files also should be noted in settingslocator.json
kubectl create configmap kube-sample-app-appsettings --from-file=./settings --from-file=../commonsettings.json