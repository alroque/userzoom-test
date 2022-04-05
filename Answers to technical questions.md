Resilience side-effects:
Sí podría retornar un 201 ya que el primary key de la tabla es event_id que se genera al vuelo cada vez que se consume el endpoint.

From the message producer standpoint:
1. Agregar una marca de tiempo a los mensajes...
2. Antes de insertar en postgresql, consultar si ese mensaje fue insertado previamente

From the message consumer standpoint:
1. Se me ocurre utilizar algún tipo de colas por consumidor...
2. Comprobar que el mensaje no haya sido procesado previamente en base de datos.

Message semantics:
 - El evento sólo contiene la información que los consumers van a necesitar, a diferencia del command (por ejemplo la propiedad items)
 - No, añade información que los consumers lo necesitan.
 - En RabbitMQ se crea otro exchange con el nombre ShipOrderCommand.