kubectl delete secret common-secrets  --ignore-not-found
kubectl create secret generic common-secrets --from-file=../secrets/

kubectl delete configmap kube-sample-app-appsettings  --ignore-not-found
kubectl create configmap kube-sample-app-appsettings --from-file=settings/
  
kubectl apply -f deployment.yaml   
kubectl apply -f service.yaml       
kubectl rollout restart deployment kube-sample-app