document.getElementById("button1").onclick = async () =>
{

    var url = "https://localhost:7217/cars?id=320";

    var car =
    {
        brand: "Renault",
        type: "Clio",
        license: "abcdefghij",
        date: 2025

    }

    var request = await fetch(url, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(car)
    })

    var response = await request.json();

    console.log(response.result);
    alert(response.message);


}

function ShowResult(response)
{
    var textContent = '';

    for (var item of response)
    {
        textContent = textContent + `
        <div class="card bg bg-primary white text-white" style="width:250px; float: left; margin: 5px; padding:5px;">
            <div class="card-body">
                <h4 class="card-title">${item.brand}</h4>
                <p class="card-text">${item.type}</p>
                 <p class="card-text">${item.license}</p>
                  <p class="card-text">${item.date}</p>
            </div>
            </div>
        `
    }

    document.getElementById('root').innerHTML = textContent;
}