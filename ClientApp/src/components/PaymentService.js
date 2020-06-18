import React, { Component } from 'react';

export class PaymentService extends Component {
    static displayName = PaymentService.name;

    constructor(props) {
        super(props);
        this.state = { payments: [], loading: true };
    }

    componentDidMount() {
        this.populatePayments();
    }

    static renderPaymentsTable(payments) {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                <tr>
                    <th>Id</th>
                    <th>Origin</th>
                    <th>Destination</th>
                    <th>Amount</th>
                    <th>Status</th>
                </tr>
                </thead>
                <tbody>
                {payments.map(payment =>
                    <tr key={payment.id}>
                        <td>{payment.id}</td>
                        <td>{payment.originAccount}</td>
                        <td>{payment.destinationAccount}</td>
                        <td>{payment.amount}</td>
                        <td>{payment.paymentStatus}</td>
                    </tr>
                )}
                </tbody>
            </table>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : PaymentService.renderPaymentsTable(this.state.payments);

        return (
            <div>
                <h1 id="tabelLabel" >Payments</h1>
                <p>This component demonstrates fetching data from a remote server.</p>
                {contents}
            </div>
        );
    }

    async populatePayments() {
        const response = await fetch('payment');
        const data = await response.json();
        this.setState({ payments: data, loading: false });
    }
}
