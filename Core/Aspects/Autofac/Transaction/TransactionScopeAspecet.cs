using System;
using System.Transactions;
using Castle.DynamicProxy;

public class TransactionScopeAspect : Attribute, IInterceptor
{
    public void Intercept(IInvocation invocation)
    {
        using (var transactionScope = new TransactionScope())
        {
            try
            {
                invocation.Proceed(); // Metodu çalıştır
                transactionScope.Complete(); // İşlem başarılı ise işlemi tamamla
            }
            catch (Exception)
            {
                // İşlem başarısız olduğunda exception fırlatır ve işlemi geri alır
                throw;
            }
        }
    }
}
