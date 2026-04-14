<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="clientes_editar.aspx.cs" Inherits="Facturador_SerinsisPC.clientes_editar" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .cliente-edit-page {
            display: flex;
            flex-direction: column;
            gap: .9rem;
        }

        .cliente-edit-header {
            display: flex;
            align-items: center;
            justify-content: space-between;
            gap: 1rem;
        }

        .cliente-edit-title {
            display: flex;
            align-items: center;
            gap: .7rem;
            color: #2a22a6;
            margin: 0;
            font-size: 2rem;
            font-weight: 700;
        }

        .cliente-edit-title i {
            color: #62afe2;
        }

        .cliente-edit-card {
            background: linear-gradient(135deg, #fbfdff 0%, #f3f9ff 100%);
            border: 1px solid #cfe2f3;
            border-radius: 18px;
            box-shadow: 0 10px 24px rgba(42, 34, 166, 0.05);
            overflow: hidden;
        }

        .cliente-edit-card__header {
            background: linear-gradient(135deg, #2a22a6 0%, #62afe2 100%);
            color: #fff;
            padding: .95rem 1rem;
        }

        .cliente-edit-card__header h2 {
            margin: 0;
            font-size: 1.2rem;
            font-weight: 700;
        }

        .cliente-edit-card__body {
            padding: 1rem;
        }

        .cliente-edit-card__body label {
            color: #2a22a6;
            font-weight: 700;
            font-size: .88rem;
            margin-bottom: .3rem;
        }

        .cliente-edit-card__body .form-control {
            border: 1px solid #cfe2f3;
            border-radius: 12px;
            min-height: 42px;
            color: #243a60;
            box-shadow: none;
        }

        .cliente-edit-card__body .form-control:focus {
            border-color: #62afe2;
            box-shadow: 0 0 0 .16rem rgba(98, 175, 226, 0.18);
        }

        .cliente-edit-actions {
            display: flex;
            justify-content: flex-end;
            gap: .6rem;
            margin-top: .8rem;
        }

        .cliente-edit-status {
            display: none;
            margin-top: .35rem;
            padding: .7rem .85rem;
            border-radius: 12px;
            font-weight: 700;
        }

        .cliente-edit-status--show {
            display: block;
        }

        .cliente-edit-status--ok {
            background: #ecfbf2;
            border: 1px solid #bde5c8;
            color: #1f7a43;
        }

        .cliente-edit-status--error {
            background: #fff4e8;
            border: 1px solid #ffd8b1;
            color: #9a4d00;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="cliente-edit-page">
        <div class="cliente-edit-header">
            <h1 class="cliente-edit-title"><i class="fas fa-user-edit"></i> <asp:Literal runat="server" ID="litTitulo" /></h1>
            <a href="clientes.aspx" class="btn btn-secondary">Volver</a>
        </div>

        <section class="cliente-edit-card">
            <div class="cliente-edit-card__header">
                <h2>Datos del cliente</h2>
            </div>
            <div class="cliente-edit-card__body">
                <div class="row">
                    <div class="col-md-6">
                        <label>Tipo Plan</label>
                        <asp:DropDownList runat="server" ID="ddl_TipoPlan" CssClass="form-control"></asp:DropDownList>
                    </div>
                    <div class="col-md-6">
                        <label>NIT.</label>
                        <asp:TextBox runat="server" ID="txtNit" CssClass="form-control" MaxLength="15" />
                    </div>
                    <div class="col-md-12">
                        <label>Nombre Comercial</label>
                        <asp:TextBox runat="server" ID="txtNombreComersial" CssClass="form-control" />
                    </div>
                    <div class="col-md-12">
                        <label>Representante</label>
                        <asp:TextBox runat="server" ID="txtRepresentante" CssClass="form-control" />
                    </div>
                    <div class="col-md-6">
                        <label>WhatsApp</label>
                        <asp:TextBox runat="server" ID="txtWhatSapp" CssClass="form-control" MaxLength="15" />
                    </div>
                    <div class="col-md-6">
                        <label>Valor</label>
                        <asp:TextBox runat="server" ID="txtValor" CssClass="form-control" TextMode="Number" />
                    </div>
                    <div class="col-md-8">
                        <label>Email</label>
                        <asp:TextBox runat="server" ID="txtCorreo" CssClass="form-control" TextMode="Email" />
                    </div>
                    <div class="col-md-4">
                        <label>Sedes</label>
                        <asp:TextBox runat="server" ID="txtSedes" CssClass="form-control" TextMode="Number" />
                    </div>
                    <div class="col-md-4">
                        <label>Dia de Pago</label>
                        <asp:TextBox runat="server" ID="txtDiaPago" CssClass="form-control" TextMode="Number" />
                    </div>
                    <div class="col-md-4">
                        <label>Inicio del Plan</label>
                        <asp:TextBox runat="server" ID="txtFechaInicioPlan" CssClass="form-control" TextMode="Date" />
                    </div>
                    <div class="col-md-4">
                        <label>Proximo Pago</label>
                        <asp:TextBox runat="server" ID="txtFechaProximoPago" CssClass="form-control" TextMode="Date" ReadOnly="true" />
                    </div>
                    <div class="col-md-12">
                        <label>Observacion de Cartera</label>
                        <asp:TextBox runat="server" ID="txtObservacionCartera" CssClass="form-control" TextMode="MultiLine" Rows="4" />
                    </div>
                    <div class="col-md-12">
                        <asp:Label runat="server" ID="lblEstadoGuardar" />
                    </div>
                </div>

                <div class="cliente-edit-actions">
                    <a href="clientes.aspx" class="btn btn-secondary">Cancelar</a>
                    <asp:Button runat="server" ID="btnGuardar" Text="Guardar" CssClass="btn btn-primary" OnClick="btnGuardar_Click" UseSubmitBehavior="false" CausesValidation="false" />
                </div>
            </div>
        </section>
    </div>

    <script>
        (function () {
            function keepOnlyDigits(element) {
                if (!element) return;
                element.value = (element.value || '').replace(/\D/g, '');
            }

            function parseIsoDate(value) {
                if (!value) return null;
                var parts = value.split('-');
                if (parts.length !== 3) return null;
                var year = parseInt(parts[0], 10);
                var month = parseInt(parts[1], 10) - 1;
                var day = parseInt(parts[2], 10);
                var date = new Date(year, month, day);
                return isNaN(date.getTime()) ? null : date;
            }

            function formatIsoDate(date) {
                var year = date.getFullYear();
                var month = String(date.getMonth() + 1).padStart(2, '0');
                var day = String(date.getDate()).padStart(2, '0');
                return year + '-' + month + '-' + day;
            }

            function daysInMonth(year, monthIndex) {
                return new Date(year, monthIndex + 1, 0).getDate();
            }

            function adjustDay(year, monthIndex, day) {
                var safeDay = Math.min(Math.max(day, 1), daysInMonth(year, monthIndex));
                return new Date(year, monthIndex, safeDay);
            }

            function calculateFirstPayment(startDate, paymentDay) {
                var calculated = adjustDay(startDate.getFullYear(), startDate.getMonth(), paymentDay);
                if (calculated < startDate) {
                    return adjustDay(startDate.getFullYear(), startDate.getMonth() + 1, paymentDay);
                }
                return calculated;
            }

            function calculateNextPayment() {
                var tipoPlan = document.getElementById('<%= ddl_TipoPlan.ClientID %>');
                var diaPago = document.getElementById('<%= txtDiaPago.ClientID %>');
                var fechaInicio = document.getElementById('<%= txtFechaInicioPlan.ClientID %>');
                var fechaProximoPago = document.getElementById('<%= txtFechaProximoPago.ClientID %>');

                if (!tipoPlan || !diaPago || !fechaInicio || !fechaProximoPago) return;

                var paymentDay = parseInt(diaPago.value, 10);
                var startDate = parseIsoDate(fechaInicio.value);
                if (!startDate || isNaN(paymentDay) || paymentDay <= 0) {
                    fechaProximoPago.value = '';
                    return;
                }

                var selectedOption = tipoPlan.options[tipoPlan.selectedIndex];
                var periodicidad = selectedOption ? parseInt(selectedOption.getAttribute('data-periodicidad') || '1', 10) : 1;
                if (isNaN(periodicidad) || periodicidad <= 0) periodicidad = 1;

                var firstPayment = calculateFirstPayment(startDate, paymentDay);
                var nextPayment = adjustDay(firstPayment.getFullYear(), firstPayment.getMonth() + periodicidad, paymentDay);
                fechaProximoPago.value = formatIsoDate(nextPayment);
            }

            function wireDigits(id) {
                var element = document.getElementById(id);
                if (!element) return;
                element.setAttribute('inputmode', 'numeric');
                element.addEventListener('input', function () { keepOnlyDigits(element); });
                keepOnlyDigits(element);
            }

            document.addEventListener('DOMContentLoaded', function () {
                ['<%= ddl_TipoPlan.ClientID %>', '<%= txtDiaPago.ClientID %>', '<%= txtFechaInicioPlan.ClientID %>'].forEach(function (id) {
                    var element = document.getElementById(id);
                    if (!element) return;
                    element.addEventListener('change', calculateNextPayment);
                    element.addEventListener('input', calculateNextPayment);
                });

                wireDigits('<%= txtNit.ClientID %>');
                wireDigits('<%= txtWhatSapp.ClientID %>');
                calculateNextPayment();
            });
        })();
    </script>
</asp:Content>
