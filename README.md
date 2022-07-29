# distributed-transaction
This repo include two solution. One of them is ChoreographyBasedSaga and another one is OrchestrationBasedSaga

We have a simple e-commerce domain which include Order, Stock and Payment service. Stock includes different three products. Stock include a few quantity of all products. 
And customer place an order which include products in stock and stock service will check which has an enough quantity of products in order if stock is not available, it will raise an OrderFailed componsable event to Order service. Otherwise stock service will raise a StockDecreased event to Payment service. And Payment service will take an payment and raise a PaymentSuccessed event to Order service also. 

ChoreographyBasedSaga solution uses outbox pattern implimentation and rabbitMQ message broker behind masstransit esb and also all consumer code is single responsibility and loosely coupled via MediatR. Also all services uses own PostgreSql database.

