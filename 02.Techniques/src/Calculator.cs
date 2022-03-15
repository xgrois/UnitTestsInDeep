namespace CalculatorLibrary;

public class Calculator
{
    public int Add(int a, int b) =>
        a + b;

    public int Sub(int a, int b) =>
        a - b;

    public int Mul(int a, int b) =>
        a * b;

    public float Div(float a, float b)
    {
        EnsureThatDivisorIsNotZero(b);
        return a / b;
    }

    private static void EnsureThatDivisorIsNotZero(float b)
    {
        if (b == 0)
            throw new DivideByZeroException();
    }
}
