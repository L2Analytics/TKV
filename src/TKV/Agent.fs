namespace TKV

module Agent =
    let New (initialState: 'state) (update: 'state -> 'message -> 'state) :Agent<'message> =
        Agent<'message>.Start(fun inbox ->
            let rec loop (currentState: 'state) = async{
                let! msg = inbox.Receive()
                let newState = update currentState msg
                return! loop newState
            }
            loop initialState
        )

    let NewMutable (initialState: 'state) (update: 'state -> 'message -> unit) :Agent<'message> =
        Agent.Start(fun inbox ->
            let currentState = initialState
            async{
             while true do
                 let! msg = inbox.Receive()
                 update currentState msg
            }
        )
