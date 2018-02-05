PromptPanel = {}
local this = PromptPanel
local panel
local prompt
local transform
local gameObject
function PromptPanel.Start()
	print("this is PromptPanel")
	panel = this.transform:GetComponent('UIPanel')
	prompt = this.transform:GetComponent('BaseLua')
	this.InitPanel()
	prompt:AddClick('Open', this.OnClick)
end

function PromptPanel.InitPanel()
	
end

function PromptPanel.OnClick(go)
	print('click from lua')
end