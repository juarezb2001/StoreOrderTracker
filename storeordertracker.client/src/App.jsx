import { useEffect, useState } from 'react';
import './App.css';

function App() {
    const [customerName, setCustomerName] = useState('');
    const [customerPhoneNumber, setCustomerPhoneNumber] = useState('');
    const [customerOrder, setCustomerOrder] = useState('');
    const [amountDue,  setAmountDue] = useState(0.00);
    const [showSuccessMessage, setShowSuccessMessage] = useState(false);
    const [showFailureMessage, setShowFailureMessage] = useState(false);

    return (
        <div>
            {showSuccessMessage ? <h1>Submission Was Successful</h1> : null}
            {showFailureMessage ? <h1>Submission Was Unsuccessful</h1> : null}
            <h1 id="tableLabel">Add Customer Order</h1>
            <label>
                Customer Name:
                <input
                    type="text"
                    value={customerName}
                    onChange={e => setCustomerName(e.target.value)}
                /> 
            </label>
            <label>
                Customer Phone Number:
                <input
                    type="text"
                    value={customerPhoneNumber}
                    onChange={e => setCustomerPhoneNumber(e.target.value)}
                />
            </label>
            <label>
                CustomerOrder:
                <input
                    type="text"
                    value={customerOrder}
                    onChange={e => setCustomerOrder(e.target.value)}
                />
            </label>
            <label>
                Amount Due:
                <input
                    type="text"
                    value={amountDue}
                    onChange={e => setAmountDue(e.target.value)}
                />
            </label>
            <button onClick={handleSubmit} disabled={customerName === ''|| customerPhoneNumber === '' || customerOrder === ''}>
                SUBMIT
            </button>
        </div>
    );
    
    async function handleSubmit() {
        let order = {
            'CustomerName': customerName,
            'CustomerPhoneNumber': customerPhoneNumber,
            'Order' : customerOrder,
            'AmountDue': amountDue,
        };
        const response = await fetch('http://localhost:5137/AddOrders', {method: 'POST', body: JSON.stringify(order), headers: { 'Content-Type': 'application/json' } });
        setCustomerName('');
        setCustomerPhoneNumber('');
        setCustomerOrder('');
        setAmountDue(0.00);
        if (response.ok) {
            setShowSuccessMessage(true);
            setTimeout(() => {
                setShowSuccessMessage(false);
                }, 3000);
        } else {
            setShowFailureMessage(true);
            setTimeout(() => {
                setShowFailureMessage(false);
            }, 3000);
        }
    }
}

export default App;