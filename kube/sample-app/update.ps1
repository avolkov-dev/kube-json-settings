## use  set -e: for bash
$ErrorActionPreference = "Stop" 

## update secrets should be outside
kubectl delete secret common-secrets  --ignore-not-found
kubectl create secret generic common-secrets --from-file=../secrets/

kubectl delete configmap kube-sample-app-appsettings  --ignore-not-found
# enumerate all needed files to config the application
# these files also should be noted in settingslocator.json
kubectl create configmap kube-sample-app-appsettings --from-file=./settings --from-file=../commonsettings.json
  
kubectl apply -f deployment.yaml   
kubectl apply -f service.yaml       

#we should not trigger restart (use https://github.com/stakater/Reloader to detect config changes)
kubectl rollout restart deployment kube-sample-app