using System;

[Serializable]
public class KahootDatos // Datos del cuestionario y sus preguntas.
{
    public string id;
    public string titulo;
    public PreguntaDatos[] preguntas;
}

// Enunciado, opciones, respuesta correcta y duración de una pregunta.
[Serializable]
public class PreguntaDatos
{
    public string enunciado;
    public string[] respuestas;
    public int respuestaCorrecta;
    public int duracionSegundos;
}