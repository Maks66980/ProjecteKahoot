using System;
using System.Collections.Generic;

// Comunica los eventos de la partida a los métodos suscritos.
public class BusEventosPartida
{
    // Agrupa los métodos suscritos por tipo de evento.
    private readonly Dictionary<Type, Delegate> _subscribers = new Dictionary<Type, Delegate>();

    // Añade un método a los suscriptores del evento.
    public void Subscribe<T>(Action<T> listener) where T : struct
    {
        Type eventType = typeof(T);

        if (_subscribers.TryGetValue(eventType, out var existingDelegate))
        {
            // Añade el método a los suscriptores existentes.
            _subscribers[eventType] = Delegate.Combine(existingDelegate, listener);
        }
        else
        {
            // Registra el primer suscriptor de este tipo de evento.
            _subscribers[eventType] = listener;
        }
    }

    // Retira un método de los suscriptores del evento.
    public void Unsubscribe<T>(Action<T> listener) where T : struct
    {
        Type eventType = typeof(T);

        if (_subscribers.TryGetValue(eventType, out var existingDelegate))
        {
            // Retira el método indicado.
            var currentDelegate = Delegate.Remove(existingDelegate, listener);

            if (currentDelegate == null)
            {
                // Elimina la entrada si ya no quedan suscriptores.
                _subscribers.Remove(eventType);
            }
            else
            {
                // Conserva los métodos que siguen suscritos.
                _subscribers[eventType] = currentDelegate;
            }
        }
    }

    // Envía el evento a todos sus suscriptores.
    public void Publish<T>(T eventData) where T : struct
    {
        Type eventType = typeof(T);

        if (_subscribers.TryGetValue(eventType, out var genericDelegate))
        {
            // Recupera el delegado con el tipo de datos del evento.
            var action = (Action<T>)genericDelegate;

            // Ejecuta los métodos suscritos con los datos recibidos.
            action.Invoke(eventData);
        }
    }
}
