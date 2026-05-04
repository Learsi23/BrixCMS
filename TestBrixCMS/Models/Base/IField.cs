namespace TestBrixCMS.Models.Base;

public interface IField
{
    // Interfaz "marcador" para que el Editor sepa qué renderizar
    string Value { get; set; }
}
