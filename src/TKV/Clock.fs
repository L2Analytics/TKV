namespace TKV


module Clock =
    type Message = 
    | Tick of AsyncReplyChannel<Time>
    | Set of Time list
    
    type State = {
        Past: Time list
    }

    let update (state: State) (msg: Message) :State =
        match msg with
        | Tick replyChannel ->
            let newTime =
                match state.Past with
                | [] -> Time.Zero ()
                | times -> Time.Successor (List.head times) 
            let updatedState = {state with Past = (newTime :: state.Past)}
            replyChannel.Reply(newTime)
            updatedState
        | Set times -> 
            {state with Past = times}
   
    type Clock () = 
        let agent = Agent.New {Past=[]} update
        member self.Tick () :Time = agent.PostAndReply(fun replyChannel -> Tick replyChannel)
        member self.Set (times: Time list) :unit = ()