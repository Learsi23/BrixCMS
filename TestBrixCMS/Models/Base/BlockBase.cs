namespace TestBrixCMS.Models.Base;

public abstract class BlockBase : brixBlockBase
{
    // Clase base para bloques individuales.
    // Todos los campos compartidos están en brixBlockBase.
}

// TU PRIMER BLOQUE DE PRUEBA
public class TextBlock : BlockBase
{
    public string Body { get; set; }
}
