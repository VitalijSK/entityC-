window.addEventListener('load', () => {
    document.querySelectorAll('.delete').forEach(btn => {
        btn.addEventListener('click', () => {
            const id = btn.getAttribute('data-id');
            const controller = btn.getAttribute('data-controller');
            fetch(`/${controller}/Delete/${id}`, {
                method: 'DELETE'
            }).then(data => {
                const container = document.querySelector('.table.container tbody');
                const item = document.querySelector(`tr[data-container-id="${id}"]`)
                container.removeChild(item);
            }).catch(e => {
                const container = document.querySelector('.table.container tbody');
                const item = document.querySelector(`tr[data-container-id="${id}"]`)
                container.removeChild(item);
            });
        })
    });

})