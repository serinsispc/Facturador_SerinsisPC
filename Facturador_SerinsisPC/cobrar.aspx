<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="cobrar.aspx.cs" Inherits="Facturador_SerinsisPC.cobrar" Async="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .cobrar-page { display: flex; flex-direction: column; gap: .9rem; }
        .cobrar-header { display: flex; align-items: center; justify-content: space-between; gap: 1rem; }
        .cobrar-header__title { display: flex; align-items: center; gap: .7rem; color: #2a22a6; margin: 0; font-size: 2rem; font-weight: 700; }
        .cobrar-header__title i { color: #62afe2; font-size: 1.7rem; }
        .cobrar-divider { height: 1px; background: linear-gradient(90deg, #c9e3f5 0%, rgba(201, 227, 245, 0) 100%); }
        .cobrar-actions { display: flex; gap: .7rem; flex-wrap: wrap; }
        .cobrar-btn { border-radius: 12px; font-weight: 700; padding: .6rem 1rem; }
        .cobrar-btn--primary { background: linear-gradient(135deg, #3c35c6 0%, #2a22a6 100%); border: 0; color: #fff; }
        .cobrar-btn--secondary { background: #eef8fe; border: 1px solid #cfe2f3; color: #2a22a6; }
        .cobrar-grid { display: grid; grid-template-columns: 1.25fr 1fr; gap: 1rem; }
        .cobrar-card { background: linear-gradient(135deg, #fbfdff 0%, #f3f9ff 100%); border: 1px solid #cfe2f3; border-radius: 16px; padding: .95rem 1rem; box-shadow: 0 10px 24px rgba(42, 34, 166, 0.05); }
        .cobrar-card__title { color: #2a22a6; font-weight: 700; font-size: .95rem; margin-bottom: .25rem; }
        .cobrar-card__text { color: #61779c; font-size: .92rem; margin-bottom: .8rem; }
        .cobrar-summary-grid { display: grid; grid-template-columns: repeat(3, minmax(0, 1fr)); gap: .85rem; }
        .cobrar-summary { background: linear-gradient(135deg, #ffffff 0%, #eef8fe 100%); border: 1px solid #d5e5f4; border-radius: 14px; padding: .8rem .9rem; }
        .cobrar-summary__label { display: block; color: #61779c; font-size: .76rem; font-weight: 700; text-transform: uppercase; letter-spacing: .04em; }
        .cobrar-summary__value { display: block; color: #2a22a6; font-size: 1.3rem; font-weight: 700; margin-top: .2rem; }
        .cobrar-summary__note { display: block; color: #61779c; font-size: .84rem; margin-top: .22rem; }
        .cobrar-filters { display: grid; grid-template-columns: repeat(4, minmax(0, 1fr)); gap: .75rem; margin-bottom: .95rem; }
        .cobrar-filters--history { grid-template-columns: repeat(3, minmax(0, 1fr)); }
        .cobrar-filter label { display: block; color: #2a22a6; font-weight: 700; font-size: .84rem; margin-bottom: .28rem; }
        .cobrar-filter .form-control { min-height: 40px; border: 1px solid #cfe2f3; border-radius: 12px; color: #243a60; box-shadow: none; }
        .cobrar-filter .form-control:focus { border-color: #62afe2; box-shadow: 0 0 0 .16rem rgba(98, 175, 226, 0.18); }
        .cobrar-filter-actions { display: flex; align-items: end; }
        .cobrar-badge { display: inline-flex; align-items: center; gap: .45rem; padding: .35rem .6rem; border-radius: 999px; font-size: .84rem; font-weight: 700; }
        .cobrar-badge--ok { background: #ecfbf2; color: #1f9b53; }
        .cobrar-badge--warn { background: #fff7e7; color: #c98916; }
        @media (max-width: 991.98px) {
            .cobrar-grid { grid-template-columns: 1fr; }
            .cobrar-summary-grid { grid-template-columns: 1fr; }
            .cobrar-filters, .cobrar-filters--history { grid-template-columns: 1fr; }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="cobrar-page">
        <section class="container-fluid">
            <div class="cobrar-header">
                <h1 class="cobrar-header__title"><i class="fas fa-money-check-alt"></i> Cobrar</h1>
                <div class="cobrar-actions">
                    <asp:LinkButton CssClass="btn cobrar-btn cobrar-btn--primary" runat="server" ID="btnCobrar" OnClick="btnCobrar_Click"><i class="fas fa-paper-plane"></i>&nbsp;Enviar Cobros</asp:LinkButton>
                    <asp:LinkButton CssClass="btn cobrar-btn cobrar-btn--secondary" runat="server" ID="btnCargarDG" OnClick="btnCargarDG_Click"><i class="fas fa-sync-alt"></i>&nbsp;Actualizar</asp:LinkButton>
                </div>
            </div>
        </section>
        <div class="cobrar-divider"></div>

        <section class="cobrar-summary-grid">
            <div class="cobrar-summary">
                <span class="cobrar-summary__label">Saldo Pendiente</span>
                <asp:Label runat="server" ID="lblResumenSaldoPendiente" CssClass="cobrar-summary__value" Text="$ 0" />
                <span class="cobrar-summary__note">Total acumulado con cartera pendiente.</span>
            </div>
            <div class="cobrar-summary">
                <span class="cobrar-summary__label">Clientes Con Saldo</span>
                <asp:Label runat="server" ID="lblResumenClientesConSaldo" CssClass="cobrar-summary__value" Text="0" />
                <span class="cobrar-summary__note">Clientes que hoy requieren seguimiento.</span>
            </div>
            <div class="cobrar-summary">
                <span class="cobrar-summary__label">Vencidos O Hoy</span>
                <asp:Label runat="server" ID="lblResumenVencidos" CssClass="cobrar-summary__value" Text="0" />
                <span class="cobrar-summary__note">Clientes con vencimiento inmediato.</span>
            </div>
        </section>

        <div class="cobrar-grid">
            <section class="cobrar-card">
                <div class="cobrar-card__title">Control de pago por cliente</div>
                <p class="cobrar-card__text">Consulta la proxima fecha de pago, el saldo pendiente acumulado y el vencimiento mas cercano.</p>
                <div class="cobrar-filters">
                    <div class="cobrar-filter">
                        <label for="<%= txtFiltroCliente.ClientID %>">Cliente o Representante</label>
                        <asp:TextBox runat="server" ID="txtFiltroCliente" CssClass="form-control" AutoPostBack="true" OnTextChanged="FiltrosControlPagosChanged" />
                    </div>
                    <div class="cobrar-filter">
                        <label for="<%= ddlFiltroPlan.ClientID %>">Plan</label>
                        <asp:DropDownList runat="server" ID="ddlFiltroPlan" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="FiltrosControlPagosChanged" />
                    </div>
                    <div class="cobrar-filter">
                        <label for="<%= ddlFiltroSaldo.ClientID %>">Saldo</label>
                        <asp:DropDownList runat="server" ID="ddlFiltroSaldo" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="FiltrosControlPagosChanged">
                            <asp:ListItem Text="Todos" Value="" />
                            <asp:ListItem Text="Solo con saldo" Value="con_saldo" />
                            <asp:ListItem Text="Solo al dia" Value="sin_saldo" />
                        </asp:DropDownList>
                    </div>
                    <div class="cobrar-filter">
                        <label for="<%= ddlFiltroVencimiento.ClientID %>">Vencimiento</label>
                        <asp:DropDownList runat="server" ID="ddlFiltroVencimiento" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="FiltrosControlPagosChanged">
                            <asp:ListItem Text="Todos" Value="" />
                            <asp:ListItem Text="Vencidos o hoy" Value="vencidos" />
                            <asp:ListItem Text="Proximos 7 dias" Value="7dias" />
                            <asp:ListItem Text="Proximos 30 dias" Value="30dias" />
                            <asp:ListItem Text="Sin fecha" Value="sin_fecha" />
                        </asp:DropDownList>
                    </div>
                </div>
                <table id="tablaControlPagos" class="table-cebra" style="width:100%;">
                    <thead>
                        <tr>
                            <th>Cliente</th>
                            <th>Plan</th>
                            <th>Dia Pago</th>
                            <th>Proximo Pago</th>
                            <th>Saldo Pendiente</th>
                            <th>Prox. Vencimiento</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater runat="server" ID="rpControlPagos">
                            <ItemTemplate>
                                <tr>
                                    <td><%# Eval("nombreComercial") %></td>
                                    <td><%# Eval("nombrePlan") %></td>
                                    <td><%# Eval("diaPago") %></td>
                                    <td><%# Eval("fechaProximoPago", "{0:yyyy-MM-dd}") %></td>
                                    <td><span class='cobrar-badge <%# Convert.ToDecimal(Eval("saldoPendienteTotal")) > 0 ? "cobrar-badge--warn" : "cobrar-badge--ok" %>'><%# $"{Eval("saldoPendienteTotal"):C0}" %></span></td>
                                    <td><%# Eval("proximoVencimiento", "{0:yyyy-MM-dd}") %></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </section>

            <section class="cobrar-card">
                <div class="cobrar-card__title">Historial de cobros enviados</div>
                <p class="cobrar-card__text">Registro de las notificaciones de cobro generadas desde la plataforma.</p>
                <div class="cobrar-filters cobrar-filters--history">
                    <div class="cobrar-filter">
                        <label for="<%= txtFiltroHistorial.ClientID %>">Cliente, representante o WhatsApp</label>
                        <asp:TextBox runat="server" ID="txtFiltroHistorial" CssClass="form-control" AutoPostBack="true" OnTextChanged="FiltrosHistorialChanged" />
                    </div>
                    <div class="cobrar-filter">
                        <label for="<%= ddlFiltroPeriodoHistorial.ClientID %>">Periodo</label>
                        <asp:DropDownList runat="server" ID="ddlFiltroPeriodoHistorial" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="FiltrosHistorialChanged">
                            <asp:ListItem Text="Todos" Value="" />
                            <asp:ListItem Text="Ultimos 7 dias" Value="7" />
                            <asp:ListItem Text="Ultimos 30 dias" Value="30" />
                            <asp:ListItem Text="Ultimos 90 dias" Value="90" />
                        </asp:DropDownList>
                    </div>
                    <div class="cobrar-filter cobrar-filter-actions">
                        <asp:LinkButton runat="server" ID="btnLimpiarFiltrosCobro" CssClass="btn cobrar-btn cobrar-btn--secondary w-100" OnClick="btnLimpiarFiltrosCobro_Click"><i class="fas fa-eraser"></i>&nbsp;Limpiar Filtros</asp:LinkButton>
                    </div>
                </div>
                <table id="tabla_cobrosEnviados" class="table-cebra" style="width:100%;">
                    <thead>
                        <tr>
                            <th>Fecha</th>
                            <th>Razon Social</th>
                            <th>Representante</th>
                            <th>WhatsApp</th>
                            <th>Sedes</th>
                            <th>Meses</th>
                            <th>Valor Cobrado</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater runat="server" ID="rpCobrosEnviados">
                            <ItemTemplate>
                                <tr>
                                    <td><%# Eval("fechaCobro", "{0:yyyy-MM-dd}") %></td>
                                    <td><%# Eval("nombreComercial") %></td>
                                    <td><%# Eval("nombreRepresentate") %></td>
                                    <td><%# Eval("celular") %></td>
                                    <td><%# Eval("sedesCobradas") %></td>
                                    <td><%# Eval("mesesCobrados") %></td>
                                    <td><%# $"{Eval("valotTotalCobrado"):C0}" %></td>
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
