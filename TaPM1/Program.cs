using System;

interface IPhoneState // интерфейс для состояний телефона
{
    void Call(string number);
    void Answer();
    void EndCall();
    void TopUpBalance(int amount);
    void WaitAnswer();
}

class WaitingState : IPhoneState // состояние "Ожидание"
{
    private readonly Phone phone;

    public WaitingState(Phone phone)
    {
        this.phone = phone;
    }

    public void Call(string number)
    {
        Console.WriteLine($"Звоним {number}");
        phone.ChangeState(new CallingState(phone));
    }

    public void Answer()
    {
        Console.WriteLine("'But nobody came'");
    }

    public void EndCall()
    {
        Console.WriteLine("Нет текущего звонка");
    }

    public void TopUpBalance(int amount)
    {
        phone.Balance += amount;
        Console.WriteLine($"Баланс пополнен - {phone.Balance}");
    }

    public void WaitAnswer()
    {
        Console.WriteLine("Вы ждете вызова");
    }
}


class CallingState : IPhoneState //  состояние звонка
{
    private readonly Phone phone;

    public CallingState(Phone phone)
    {
        this.phone = phone;
    }

    public void Call(string number)
    {
        Console.WriteLine("Нельзя звонить во время вызова");
    }

    public void Answer()
    {
        Console.WriteLine("Нельзя ответить во время вызова");
    }

    public void EndCall()
    {
        Console.WriteLine("Завершение звонка");
        phone.ChangeState(new WaitingState(phone));
    }

    public void TopUpBalance(int amount)
    {
        Console.WriteLine("Нельзя пополнить баланс во время вызова");
    }

    public void WaitAnswer()
    {
        Random random = new Random();
        if (random.Next(1, 2) == 1)
        {
            Console.WriteLine("Обонент ответил");
            phone.ChangeState(new TalkingState(phone));
        }
        else
        {
            Console.WriteLine("Нет ответа, завершение звонка");
            phone.ChangeState(new WaitingState(phone));
        }
    }
}


class TalkingState : IPhoneState //  состояние разговора
{
    private readonly Phone phone;

    public TalkingState(Phone phone)
    {
        this.phone = phone;
    }

    public void Call(string number)
    {
        Console.WriteLine("Нельзя позвонить во время разговора");
    }

    public void Answer()
    {
        Console.WriteLine("Нельзя ответить во время разговора");
    }

    public void EndCall()
    {
        Console.WriteLine("Завершение звонка");
        phone.ChangeState(new WaitingState(phone));
    }

    public void TopUpBalance(int amount)
    {
        Console.WriteLine("Нельзя пополнить баланс во время звонка");
    }

    public void WaitAnswer()
    {
        Console.WriteLine("Зачем ждать вызова во время звонка?");
    }
}


class Phone // класс телефона
{
    private IPhoneState currentState;

    public Phone()
    {
        currentState = new WaitingState(this);
    }

    public int Balance { get; set; }

    public void Call(string number)
    {
        currentState.Call(number);
    }

    public void Answer()
    {
        currentState.Answer();
    }

    public void EndCall()
    {
        currentState.EndCall();
    }

    public void TopUpBalance(int amount)
    {
        currentState.TopUpBalance(amount);
    }

    public void WaitAnswer()
    {
        currentState.WaitAnswer();
    }

    public void ChangeState(IPhoneState newState)
    {
        currentState = newState;
    }
}

class Program
{
    static void Main()
    {
        // создавем телефон
        Phone phone = new Phone();
                
        // звоним васе
        phone.Call("123-456-789");
        // ждем ответа
        phone.WaitAnswer();
        // дальше зависимости от ответа
        // пытаемся ответить во время разговора
        phone.Answer();
        // пополняем баланс
        phone.TopUpBalance(50);
        // пытаемся завершить  развговор
        phone.EndCall();

        // тестим состояние ожидания
        Console.WriteLine("Состояние ожидания");
        phone.ChangeState(new WaitingState(phone));
        // пытаемся ответить на пустой звонок
        phone.Answer();
        // пополняем баланс
        phone.TopUpBalance(50);
        // ждать вызова
        phone.WaitAnswer();
        // пытаемся завершить пустой звонок
        phone.EndCall();

        // тестим состояние звонка
        Console.WriteLine("Состояние звонка");
        phone.ChangeState(new CallingState(phone));
        // пытаемся ответить на пустой звонок
        phone.Answer();
        // пополняем баланс
        phone.TopUpBalance(50);
        // ждать вызова
        phone.WaitAnswer();
        // пытаемся завершить пустой звонок
        phone.EndCall();

        // тестим состояние разговора
        Console.WriteLine("Состояние разговора");
        phone.ChangeState(new TalkingState(phone));
        // пытаемся ответить на пустой звонок
        phone.Answer();
        // пополняем баланс
        phone.TopUpBalance(50);
        // ждать вызова
        phone.WaitAnswer();
        // пытаемся завершить пустой звонок
        phone.EndCall();
    }
}
