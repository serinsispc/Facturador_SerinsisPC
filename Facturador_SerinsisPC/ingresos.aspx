<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="ingresos.aspx.cs" Inherits="Facturador_SerinsisPC.ingresos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .ingresos-page { display: flex; flex-direction: column; gap: .9rem; }
        .ingresos-header { display: flex; align-items: center; justify-content: space-between; gap: 1rem; }
        .ingresos-header__title { display: flex; align-items: center; gap: .7rem; color: #2a22a6; margin: 0; font-size: 2rem; font-weight: 700; }
        .ingresos-header__title i { color: #62afe2; font-size: 1.7rem; }
        .ingresos-divider { height: 1px; background: linear-gradient(90deg, #c9e3f5 0%, rgba(201, 227, 245, 0) 100%); }
        .ingresos-grid { display: grid; grid-template-columns: minmax(320px, .9fr) minmax(520px, 1.4fr); gap: 1rem; }
        .ingresos-card { background: linear-gradient(135deg, #fbfdff 0%, #f3f9ff 100%); border: 1px solid #cfe2f3; border-radius: 16px; padding: .95rem 1rem; box-shadow: 0 10px 24px rgba(42, 34, 166, 0.05); }
        .ingresos-card__title { color: #2a22a6; font-weight: 700; font-size: .95rem; margin-bottom: .25rem; }
        .ingresos-card__text { color: #61779c; font-size: .92rem; margin-bottom: .8rem; }
        @media (max-width: 991.98px) { .ingresos-grid { grid-template-columns: 1fr; } }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="ingresos-page">
        <section class="container-fluid">
            <div class="ingresos-header">
                <h1 class="ingresos-header__title"><i class="fas fa-chart-line"></i> Ingresos</h1>
            </div>
        </section>
        <div class="ingresos-divider"></div>

        <div class="ingresos-grid">
            <section class="ingresos-card">
                <div class="ingresos-card__title">Ingresos por mes</div>
                <p class="ingresos-card__text">Resumen del dinero realmente recibido, agrupado por anio y mes.</p>
                <table id="tablaIngresosMensuales" class="table-cebra" style="width:100%;">
                    <thead>
                        <tr>
                            <th>Anio</th>
                            <th>Mes</th>
                            <th>Cantidad Pagos</th>
                            <th>Total Ingresos</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater runat="server" ID="rpIngresosMensuales">
                            <ItemTemplate>
                                <tr>
                                    <td><%# Eval("anio") %></td>
                                    <td><%# Eval("mes") %></td>
                                    <td><%# Eval("cantidadPagos") %></td>
                                    <td><%# $"{Eval("totalIngresos"):C0}" %></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </section>

            <section class="ingresos-card">
                <div class="ingresos-card__title">Pagos recibidos</div>
                <p class="ingresos-card__text">Detalle de pagos reales registrados con su metodo, comprobante y cliente.</p>
                <table id="tablaPagosRecibidos" class="table-cebra" style="width:100%;">
                    <thead>
                        <tr>
                            <th>Fecha</th>
                            <th>Cliente</th>
                            <th>Metodo</th>
                            <th>Valor</th>
                            <th>Comprobante</th>
                            <th>Referencia</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater runat="server" ID="rpPagosRecibidos">
                            <ItemTemplate>
                                <tr>
                                    <td><%# Eval("fechaPago", "{0:yyyy-MM-dd}") %></td>
                                    <td><%# Eval("nombreComercial") %></td>
                                    <td><%# Eval("nombreMetodo") %></td>
                                    <td><%# $"{Eval("valorRecibido"):C0}" %></td>
                                    <td><%# Eval("numeroComprobante") %></td>
                                    <td><%# Eval("referenciaPago") %></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </section>
        </div>
    </div>
    <script src="js/myScripts.js"></script>
</asp:Content>
