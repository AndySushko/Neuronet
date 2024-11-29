
namespace WindowsFormsApp1.NeyroNet
{
    enum NeuronType
    {
        Hidden,//нейрон скрытого слоя
        Output//нейрон выходного слоя
    }

    enum NetworkMode
    {
        Train,//режим обучения
        Test,//режим тестирования
        Recogn//режим распознавания
    }
    enum MemoryMode
    {
        GET,//считывние
        SET,//сохранение
        INIT//инициализация
    }
}
