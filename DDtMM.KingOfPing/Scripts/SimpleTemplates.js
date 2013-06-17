var simpleTemplates = (function () {
    var templateParseRegex = /\{\{(?:([^:\}]*):)*([^}]+)\}\}/g;

    var field = function (dataKey, functionId, startIndex, length) {
        this.dataKey = dataKey;
        this.functionId = functionId;
        this.startIndex = startIndex;
        this.endIndex = startIndex + length;
        this.length = length;
    }
    var templateDefinition = function (source) {
        this.fields = [];
        this.dataKeys = [];
        this.source = source;

        this.parseSource();
    };

    (function () {
        this.parseSource = function() {
            var match, dataKey;

            this.dataKeys = [];
            this.fields = [];
            while ((match = templateParseRegex.exec(this.source)) !== null) {
                dataKey = match[2];
                if (this.dataKeys.indexOf(dataKey) < 0) {
                    this.dataKeys.push(dataKey);
                };
                this.fields.push(new field(dataKey, match[1], match.index, match[0].length));
            }
        };
    }).call(templateDefinition.prototype);

    function renderField(field, data) {
 
        if (data != null) {
            if (field.functionId != null) {
                if ((subFunction = my.functions[field.functionId]) != null) {
                    data = subFunction(data);
                }
            }
        } else {
            data = '';
        }
        console.log(['FIELD: ' , data]);
        return data;
    }

    function renderTemplate(data, template) {

        var subFunction,
            lastIndex = 0,
            field,
            output = '',
            dataType = Object.prototype.toString.call(data);

        switch (dataType) {
            case '[object Object]':
                for (var i = 0, il = template.fields.length; i < il; i++) {

                    field = template.fields[i];
                    console.log(['OBJECT: ', field]);
                    output += template.source.substr(lastIndex, field.startIndex) +
                        renderField(field, data[field.dataKey]);
                    lastIndex = field.endIndex;
                }
                output + template.source.substr(lastIndex);
                console.log('OBJECT OUT: ', output);
            case '[object Array]':
                field = template.fields[0];
                console.log(['Array: ', field]);
                for (var i = 0, il = data.length; i < il; i++) {
                    output += renderField(field, data[i]);
                }
            default:
                
                console.log(['string?: ', template.fields[0]]);
                output = renderField(template.fields[0], data);

        }
        console.log(['out: ', output]);
        return output;
    };

    function scanForTemplates() {
        var id;
        $('script[type="text/template"]').each(function () {
            id = this.id;
            var template = new templateDefinition(this.innerHTML);
            my.templates[id] = template;
            my.functions[id] = function (data) {
                return renderTemplate(data, template);
            }
        });
    }


    var my = {
        // jQuery element representing the slider.  Use setSlider to set
        functions: {},
        templates: {},
        scanForTemplates: scanForTemplates,
        renderTemplate: renderTemplate,
    }

    return my;

})();