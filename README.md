First & foremost, I Would like to thank https://github.com/binarythistle. 
this is an updated .Net 6 version app of the tutorial https://youtu.be/DgVjEo3OGBI?si=dU2EoPhO7bE2xhEG.

Two Independent Microservices with Nginx gateway/Loadbalancer used to communicate synchronously by HTTP & Grpc
Asynchronously by Rabbitmq eventual consistency.

These Microservice APIs are built with a Repository pattern in .Net Core 6 and deployed in Kubernetes with the help of docker desktop.

Helpful Commands:
#docker build -t codebind007/commandservice .
#docker build -t codebind007/platformservice .
#kubectl get deployments
#kubectl get pods
#kubectl get services
#kubectl rollout restart deployment <deployment>
#docker push <container>

They communicate through 2 types 
1) Synchronously :- Https , Grpc (Http2 without TLS)
2) Asynchronously:- RabbitMQ (Eventual consistency)

Architecture:

![Architecture](https://github.com/pavan296/Microservices-with-K8s/assets/57304079/4d1fc489-4ba5-4650-bcff-fda43ba0949e)








