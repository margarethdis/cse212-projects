public static class MysteryStack1 { //aqui define la clase 

    //recordar que static significa que no necesitas crear un 
    // objeto para usarla 

    //esta clase contiene una opcion run que puedo usar facilmente

    //public: puede ser llamada desde fuera de la clase 
    //static: igap que arriba se usa sin crear una instancia. 



    public static string Run(string text)  //la funcion devuelve una 
    //cadena y toma como entrada una cadena llamada text.
    {
        var stack = new Stack<char>();
        foreach (var letter in text)
            stack.Push(letter);

        var result = "";
        while (stack.Count > 0)
            result += stack.Pop();

        return result;
    }
}