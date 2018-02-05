GameManager = {};
function GameManager.LuaScriptPanel()
	return 'Prompt', 'Message';
end

function GameManager.OnInitOK()
	Framework.ioo.panelManager:CreatePanel("Prompt")
end

function GameManager.Start()
end

function GameManager.OnClick(go)
end