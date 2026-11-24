using System.IO;

// Comprueba que el cuestionario tenga datos válidos para jugar.
public class ValidadorKahoots
{
    public void Validar(KahootDatos datos) // Valida el cuestionario y sus preguntas.
    {
        if (datos == null) // Comprueba que haya datos del cuestionario.
        {
            throw new InvalidDataException(
                "El archivo no contiene un cuestionario.");
        }
        else if (string.IsNullOrWhiteSpace(datos.id) ||
                 string.IsNullOrWhiteSpace(datos.titulo)) // Comprueba el ID y el título.
        {
            throw new InvalidDataException(
                "El cuestionario necesita un ID y un título."); // El ID y el título son obligatorios.
        }
        else if (datos.preguntas == null ||
                 datos.preguntas.Length == 0) // Comprueba que haya preguntas.
        {
            throw new InvalidDataException(
                "El cuestionario no contiene preguntas."); // El cuestionario necesita al menos una pregunta.
        }
        else
        {
            for (int i = 0; i < datos.preguntas.Length; i++)
            {
                // El mensaje utiliza posiciones desde 1 para identificar la pregunta.
                ValidarPregunta(datos.preguntas[i], i + 1);
            }
        }
    }

    private void ValidarPregunta(PreguntaDatos pregunta, int numero)
    {
        string mensaje = "Pregunta " + numero + ": ";

        if (pregunta == null)
        {
            throw new InvalidDataException(mensaje + "faltan los datos."); // Faltan los datos de la pregunta.
        }
        else if (string.IsNullOrWhiteSpace(pregunta.enunciado))
        {
            throw new InvalidDataException(mensaje + "falta el enunciado."); // El enunciado es obligatorio.
        }
        else if (pregunta.respuestas == null ||
                 pregunta.respuestas.Length != 4) // Comprueba que haya cuatro opciones.
        {
            // La interfaz de partida está diseñada con cuatro opciones.
            throw new InvalidDataException(
                mensaje + "debe tener cuatro respuestas.");
        }
        else if (pregunta.respuestaCorrecta < 0 ||
                 pregunta.respuestaCorrecta >= pregunta.respuestas.Length) // Comprueba el rango del índice de la respuesta correcta.
        {
            // El índice de la respuesta correcta empieza en cero.
            throw new InvalidDataException(
                mensaje + "el índice de la respuesta correcta no es válido.");
        }
        else if (pregunta.duracionSegundos <= 0) // Rechaza las duraciones iguales o inferiores a cero.
        {
            throw new InvalidDataException(
                mensaje + "la duración debe ser mayor que cero.");
        }
        else
        {
            foreach (string respuesta in pregunta.respuestas)
            {
                if (string.IsNullOrWhiteSpace(respuesta))
                {
                    throw new InvalidDataException(
                        mensaje + "hay una respuesta vacía.");
                }
            }
        }
    }
}
