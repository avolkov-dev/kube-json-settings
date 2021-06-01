## use  set -e: for bash
$ErrorActionPreference = "Stop" 

## update secrets should be outside
kubectl delete secret common-secrets  --ignore-not-found
kubectl create secret generic common-secrets --from-file=../secrets/

./settings/build-configmap.ps1
  
kubectl apply -f deployment.yaml   
kubectl apply -f service.yaml       

#we should not trigger restart (use https://github.com/stakater/Reloader to detect config changes)
kubectl rollout restart deployment kube-sample-app