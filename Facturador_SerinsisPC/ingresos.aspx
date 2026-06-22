<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="ingresos.aspx.cs" Inherits="Facturador_SerinsisPC.ingresos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .ingresos-page { display: flex; flex-direction: column; gap: .9rem; }
        .ingresos-page .container-fluid { padding-left: .2rem; padding-right: .2rem; }
        .ingresos-header { display: flex; align-items: center; justify-content: space-between; gap: 1rem; }
        .ingresos-header__title { display: flex; align-items: center; gap: .7rem; color: #2a22a6; margin: 0; font-size: 2rem; font-weight: 700; }
        .ingresos-header__title i { color: #62afe2; font-size: 1.7rem; }
        .ingresos-divider { height: 1px; background: linear-gradient(90deg, #c9e3f5 0%, rgba(201, 227, 245, 0) 100%); }
        .ingresos-grid { display: grid; grid-template-columns: minmax(320px, .9fr) minmax(520px, 1.4fr); gap: 1rem; }
        .ingresos-card { background: linear-gradient(135deg, #fbfdff 0%, #f3f9ff 100%); border: 1px solid #cfe2f3; border-radius: 16px; padding: .95rem 1rem; box-shadow: 0 10px 24px rgba(42, 34, 166, 0.05); }
        .ingresos-card__title { color: #2a22a6; font-weight: 700; font-size: .95rem; margin-bottom: .25rem; }
        .ingresos-card__text { color: #61779c; font-size: .92rem; margin-bottom: .8rem; }
        .ingresos-table-wrap { overflow-x: auto; -webkit-overflow-scrolling: touch; }
        .ingresos-table-wrap .dataTables_wrapper { width: 100%; min-width: 100%; }
        .ingresos-table { width: 100% !important; min-width: 680px; }
        .ingresos-table--wide { min-width: 980px; }
        .pagos-gestion-card { background: linear-gradient(135deg, #fbfdff 0%, #f3f9ff 100%); border: 1px solid #cfe2f3; border-radius: 16px; padding: .95rem 1rem; box-shadow: 0 10px 24px rgba(42, 34, 166, 0.05); }
        .pagos-filters { display: grid; grid-template-columns: minmax(220px, 1.2fr) minmax(180px, .7fr) minmax(180px, .7fr) auto; gap: .75rem; margin-bottom: .95rem; }
        .pagos-filter label { display: block; color: #2a22a6; font-weight: 700; font-size: .84rem; margin-bottom: .28rem; }
        .pagos-filter .form-control { min-height: 40px; border: 1px solid #cfe2f3; border-radius: 12px; color: #243a60; box-shadow: none; }
        .pagos-filter .form-control:focus { border-color: #62afe2; box-shadow: 0 0 0 .16rem rgba(98, 175, 226, 0.18); }
        .pagos-filter-actions { display: flex; align-items: end; }
        .pagos-action { display: inline-flex; align-items: center; justify-content: center; width: 38px; height: 38px; border-radius: 11px; text-decoration: none; transition: transform .15s ease, box-shadow .15s ease; }
        .pagos-action:hover { text-decoration: none; transform: translateY(-1px); box-shadow: 0 8px 16px rgba(42, 34, 166, 0.10); }
        .pagos-action--delete { background: #fff0f2; color: #d8344a; }
        .pagos-empty { color: #61779c; font-size: .92rem; margin: .4rem 0 0; }
        @media (max-width: 991.98px) {
            .ingresos-header {
                flex-direction: column;
                align-items: flex-start;
            }

            .ingresos-grid { grid-template-columns: 1fr; }
            .pagos-filters { grid-template-columns: 1fr 1fr; }
            .ingresos-table { min-width: 640px; }
            .ingresos-table--wide { min-width: 900px; }
        }

        @media (max-width: 767.98px) {
            .ingresos-header__title {
                font-size: 1.6rem;
            }

            .pagos-filters { grid-template-columns: 1fr; }
            .ingresos-card,
            .pagos-gestion-card { padding: .9rem .75rem; }
            .ingresos-table { min-width: 620px; }
            .ingresos-table--wide { min-width: 860px; }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="ingresos-page">
        <section class="container-fluid">
            <div class="ingresos-header">
                <h1 class="ingresos-header__title"><i class="fas fa-chart-line"></i> Ingresos y Pagos</h1>
            </div>
        </section>
        <div class="ingresos-divider"></div>

        <div class="ingresos-grid">
            <section class="ingresos-card">
                <div class="ingresos-card__title">Ingresos por mes</div>
                <p class="ingresos-card__text">Resumen del dinero realmente recibido, agrupado por anio y mes.</p>
                <div class="ingresos-table-wrap">
                    <table id="tablaIngresosMensuales" class="table-cebra ingresos-table" style="width:100%;">
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
                </div>
            </section>

            <section class="ingresos-card">
                <div class="ingresos-card__title">Pagos recibidos</div>
                <p class="ingresos-card__text">Historial general de pagos registrados con su metodo, comprobante y cliente.</p>
                <div class="ingresos-table-wrap">
                    <table id="tablaPagosRecibidos" class="table-cebra ingresos-table" style="width:100%;">
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
                </div>
            </section>
        </div>

        <section class="pagos-gestion-card">
            <div class="ingresos-card__title">Gestion de pagos reportados</div>
            <p class="ingresos-card__text">Filtra, revisa y elimina pagos mal reportados para reactivar el cobro correcto del cliente que realmente sigue debiendo.</p>

            <div class="pagos-filters">
                <div class="pagos-filter">
                    <label for="<%= txtFiltroPago.ClientID %>">Cliente, representante, referencia o comprobante</label>
                    <asp:TextBox runat="server" ID="txtFiltroPago" CssClass="form-control" AutoPostBack="true" OnTextChanged="FiltrosPagoChanged" />
                </div>
                <div class="pagos-filter">
                    <label for="<%= ddlFiltroMetodoPago.ClientID %>">Metodo</label>
                    <asp:DropDownList runat="server" ID="ddlFiltroMetodoPago" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="FiltrosPagoChanged" />
                </div>
                <div class="pagos-filter">
                    <label for="<%= ddlFiltroPeriodoPago.ClientID %>">Periodo</label>
                    <asp:DropDownList runat="server" ID="ddlFiltroPeriodoPago" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="FiltrosPagoChanged">
                        <asp:ListItem Text="Todos" Value="" />
                        <asp:ListItem Text="Hoy" Value="hoy" />
                        <asp:ListItem Text="Ultimos 7 dias" Value="7" />
                        <asp:ListItem Text="Ultimos 30 dias" Value="30" />
                        <asp:ListItem Text="Ultimos 90 dias" Value="90" />
                    </asp:DropDownList>
                </div>
                <div class="pagos-filter pagos-filter-actions">
                    <asp:LinkButton runat="server" ID="btnLimpiarFiltrosPago" CssClass="btn btn-outline-primary w-100" OnClick="btnLimpiarFiltrosPago_Click"><i class="fas fa-eraser"></i>&nbsp;Limpiar</asp:LinkButton>
                </div>
            </div>

            <div class="ingresos-table-wrap">
                <table id="tablaPagosGestion" class="table-cebra ingresos-table ingresos-table--wide" style="width:100%;">
                    <thead>
                        <tr>
                            <th>Fecha</th>
                            <th>Cliente</th>
                            <th>Representante</th>
                            <th>Metodo</th>
                            <th>Valor</th>
                            <th>Comprobante</th>
                            <th>Referencia</th>
                            <th>Observacion</th>
                            <th>Opcion</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater runat="server" ID="rpPagosGestion">
                            <ItemTemplate>
                                <tr>
                                    <td><%# Eval("fechaPago", "{0:yyyy-MM-dd}") %></td>
                                    <td><%# Eval("nombreComercial") %></td>
                                    <td><%# Eval("nombreRepresentate") %></td>
                                    <td><%# Eval("nombreMetodo") %></td>
                                    <td><%# $"{Eval("valorRecibido"):C0}" %></td>
                                    <td><%# Eval("numeroComprobante") %></td>
                                    <td><%# Eval("referenciaPago") %></td>
                                    <td><%# Eval("observacion") %></td>
                                    <td class="text-center">
                                        <asp:LinkButton runat="server" ID="btnEliminarPago" CssClass="pagos-action pagos-action--delete" ToolTip="Eliminar pago reportado" OnClick="btnEliminarPago_Click" OnClientClick="return confirm('Se eliminara este pago y se reactivara el cobro asociado. ¿Deseas continuar?');" CommandArgument='<%# Eval("id") %>'><i class="fas fa-trash-alt"></i></asp:LinkButton>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </div>
            <asp:Label runat="server" ID="lblSinPagos" CssClass="pagos-empty" Visible="false" Text="No se encontraron pagos con los filtros aplicados." />
        </section>
    </div>
    <script src="js/myScripts.js"></script>
</asp:Content>
