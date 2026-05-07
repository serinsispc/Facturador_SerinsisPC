using MensualidadesSerinsisPC.V2.Application.DTOs;

namespace MensualidadesSerinsisPC.V2.Application.Abstractions;

public interface IPaymentOperationsService
{
    Task<IReadOnlyList<LookupOptionDto>> GetPaymentMethodOptionsAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<InvoiceListItemDto>> GetPendingInvoicesAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PaymentReceiptListItemDto>> GetRecentPaymentsAsync(CancellationToken cancellationToken = default);
    Task RegisterPaymentAsync(PaymentRegisterDto request, string registeredBy, CancellationToken cancellationToken = default);
}
