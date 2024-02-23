const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .build();

connection.on("ReceiveMessage", (user, message) => {
    console.log(`${user}: ${message}`);
});

connection.start()
    .then(() => {
        console.log("Connection established.");
    })
    .catch(err => console.error(err));

const sendMessage = (user, message) => {
    connection.invoke("SendMessage", user, message)
        .catch(err => console.error(err));
}
