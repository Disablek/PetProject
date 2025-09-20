import {
  __commonJS
} from "./chunk-WOOG5QLI.js";

// node_modules/@babel/runtime/helpers/interopRequireDefault.js
var require_interopRequireDefault = __commonJS({
  "node_modules/@babel/runtime/helpers/interopRequireDefault.js"(exports, module) {
    function _interopRequireDefault(e) {
      return e && e.__esModule ? e : {
        "default": e
      };
    }
    module.exports = _interopRequireDefault, module.exports.__esModule = true, module.exports["default"] = module.exports;
  }
});

// node_modules/rc-pagination/lib/locale/ru_RU.js
var require_ru_RU = __commonJS({
  "node_modules/rc-pagination/lib/locale/ru_RU.js"(exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", {
      value: true
    });
    exports.default = void 0;
    var locale = {
      // Options
      items_per_page: "/ стр.",
      jump_to: "Перейти",
      jump_to_confirm: "подтвердить",
      page: "Страница",
      // Pagination
      prev_page: "Назад",
      next_page: "Вперед",
      prev_5: "Предыдущие 5",
      next_5: "Следующие 5",
      prev_3: "Предыдущие 3",
      next_3: "Следующие 3",
      page_size: "размер страницы"
    };
    var _default = exports.default = locale;
  }
});

// node_modules/@babel/runtime/helpers/typeof.js
var require_typeof = __commonJS({
  "node_modules/@babel/runtime/helpers/typeof.js"(exports, module) {
    function _typeof(o) {
      "@babel/helpers - typeof";
      return module.exports = _typeof = "function" == typeof Symbol && "symbol" == typeof Symbol.iterator ? function(o2) {
        return typeof o2;
      } : function(o2) {
        return o2 && "function" == typeof Symbol && o2.constructor === Symbol && o2 !== Symbol.prototype ? "symbol" : typeof o2;
      }, module.exports.__esModule = true, module.exports["default"] = module.exports, _typeof(o);
    }
    module.exports = _typeof, module.exports.__esModule = true, module.exports["default"] = module.exports;
  }
});

// node_modules/@babel/runtime/helpers/toPrimitive.js
var require_toPrimitive = __commonJS({
  "node_modules/@babel/runtime/helpers/toPrimitive.js"(exports, module) {
    var _typeof = require_typeof()["default"];
    function toPrimitive(t, r) {
      if ("object" != _typeof(t) || !t) return t;
      var e = t[Symbol.toPrimitive];
      if (void 0 !== e) {
        var i = e.call(t, r || "default");
        if ("object" != _typeof(i)) return i;
        throw new TypeError("@@toPrimitive must return a primitive value.");
      }
      return ("string" === r ? String : Number)(t);
    }
    module.exports = toPrimitive, module.exports.__esModule = true, module.exports["default"] = module.exports;
  }
});

// node_modules/@babel/runtime/helpers/toPropertyKey.js
var require_toPropertyKey = __commonJS({
  "node_modules/@babel/runtime/helpers/toPropertyKey.js"(exports, module) {
    var _typeof = require_typeof()["default"];
    var toPrimitive = require_toPrimitive();
    function toPropertyKey(t) {
      var i = toPrimitive(t, "string");
      return "symbol" == _typeof(i) ? i : i + "";
    }
    module.exports = toPropertyKey, module.exports.__esModule = true, module.exports["default"] = module.exports;
  }
});

// node_modules/@babel/runtime/helpers/defineProperty.js
var require_defineProperty = __commonJS({
  "node_modules/@babel/runtime/helpers/defineProperty.js"(exports, module) {
    var toPropertyKey = require_toPropertyKey();
    function _defineProperty(e, r, t) {
      return (r = toPropertyKey(r)) in e ? Object.defineProperty(e, r, {
        value: t,
        enumerable: true,
        configurable: true,
        writable: true
      }) : e[r] = t, e;
    }
    module.exports = _defineProperty, module.exports.__esModule = true, module.exports["default"] = module.exports;
  }
});

// node_modules/@babel/runtime/helpers/objectSpread2.js
var require_objectSpread2 = __commonJS({
  "node_modules/@babel/runtime/helpers/objectSpread2.js"(exports, module) {
    var defineProperty = require_defineProperty();
    function ownKeys(e, r) {
      var t = Object.keys(e);
      if (Object.getOwnPropertySymbols) {
        var o = Object.getOwnPropertySymbols(e);
        r && (o = o.filter(function(r2) {
          return Object.getOwnPropertyDescriptor(e, r2).enumerable;
        })), t.push.apply(t, o);
      }
      return t;
    }
    function _objectSpread2(e) {
      for (var r = 1; r < arguments.length; r++) {
        var t = null != arguments[r] ? arguments[r] : {};
        r % 2 ? ownKeys(Object(t), true).forEach(function(r2) {
          defineProperty(e, r2, t[r2]);
        }) : Object.getOwnPropertyDescriptors ? Object.defineProperties(e, Object.getOwnPropertyDescriptors(t)) : ownKeys(Object(t)).forEach(function(r2) {
          Object.defineProperty(e, r2, Object.getOwnPropertyDescriptor(t, r2));
        });
      }
      return e;
    }
    module.exports = _objectSpread2, module.exports.__esModule = true, module.exports["default"] = module.exports;
  }
});

// node_modules/rc-picker/lib/locale/common.js
var require_common = __commonJS({
  "node_modules/rc-picker/lib/locale/common.js"(exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", {
      value: true
    });
    exports.commonLocale = void 0;
    var commonLocale = exports.commonLocale = {
      yearFormat: "YYYY",
      dayFormat: "D",
      cellMeridiemFormat: "A",
      monthBeforeYear: true
    };
  }
});

// node_modules/rc-picker/lib/locale/ru_RU.js
var require_ru_RU2 = __commonJS({
  "node_modules/rc-picker/lib/locale/ru_RU.js"(exports) {
    "use strict";
    var _interopRequireDefault = require_interopRequireDefault().default;
    Object.defineProperty(exports, "__esModule", {
      value: true
    });
    exports.default = void 0;
    var _objectSpread2 = _interopRequireDefault(require_objectSpread2());
    var _common = require_common();
    var locale = (0, _objectSpread2.default)((0, _objectSpread2.default)({}, _common.commonLocale), {}, {
      locale: "ru_RU",
      today: "Сегодня",
      now: "Сейчас",
      backToToday: "Текущая дата",
      ok: "ОК",
      clear: "Очистить",
      week: "Неделя",
      month: "Месяц",
      year: "Год",
      timeSelect: "Выбрать время",
      dateSelect: "Выбрать дату",
      monthSelect: "Выбрать месяц",
      yearSelect: "Выбрать год",
      decadeSelect: "Выбрать десятилетие",
      dateFormat: "D-M-YYYY",
      dateTimeFormat: "D-M-YYYY HH:mm:ss",
      previousMonth: "Предыдущий месяц (PageUp)",
      nextMonth: "Следующий месяц (PageDown)",
      previousYear: "Предыдущий год (Control + left)",
      nextYear: "Следующий год (Control + right)",
      previousDecade: "Предыдущее десятилетие",
      nextDecade: "Следущее десятилетие",
      previousCentury: "Предыдущий век",
      nextCentury: "Следующий век"
    });
    var _default = exports.default = locale;
  }
});

// node_modules/antd/lib/time-picker/locale/ru_RU.js
var require_ru_RU3 = __commonJS({
  "node_modules/antd/lib/time-picker/locale/ru_RU.js"(exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", {
      value: true
    });
    exports.default = void 0;
    var locale = {
      placeholder: "Выберите время",
      rangePlaceholder: ["Время начала", "Время окончания"]
    };
    var _default = exports.default = locale;
  }
});

// node_modules/antd/lib/date-picker/locale/ru_RU.js
var require_ru_RU4 = __commonJS({
  "node_modules/antd/lib/date-picker/locale/ru_RU.js"(exports) {
    "use strict";
    var _interopRequireDefault = require_interopRequireDefault().default;
    Object.defineProperty(exports, "__esModule", {
      value: true
    });
    exports.default = void 0;
    var _ru_RU = _interopRequireDefault(require_ru_RU2());
    var _ru_RU2 = _interopRequireDefault(require_ru_RU3());
    var locale = {
      lang: Object.assign({
        placeholder: "Выберите дату",
        yearPlaceholder: "Выберите год",
        quarterPlaceholder: "Выберите квартал",
        monthPlaceholder: "Выберите месяц",
        weekPlaceholder: "Выберите неделю",
        rangePlaceholder: ["Начальная дата", "Конечная дата"],
        rangeYearPlaceholder: ["Начальный год", "Год окончания"],
        rangeMonthPlaceholder: ["Начальный месяц", "Конечный месяц"],
        rangeWeekPlaceholder: ["Начальная неделя", "Конечная неделя"],
        shortWeekDays: ["Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб"],
        shortMonths: ["Янв", "Фев", "Мар", "Апр", "Май", "Июн", "Июл", "Авг", "Сен", "Окт", "Ноя", "Дек"]
      }, _ru_RU.default),
      timePickerLocale: Object.assign({}, _ru_RU2.default)
    };
    var _default = exports.default = locale;
  }
});

// node_modules/antd/lib/calendar/locale/ru_RU.js
var require_ru_RU5 = __commonJS({
  "node_modules/antd/lib/calendar/locale/ru_RU.js"(exports) {
    "use strict";
    var _interopRequireDefault = require_interopRequireDefault().default;
    Object.defineProperty(exports, "__esModule", {
      value: true
    });
    exports.default = void 0;
    var _ru_RU = _interopRequireDefault(require_ru_RU4());
    var _default = exports.default = _ru_RU.default;
  }
});

// node_modules/antd/lib/locale/ru_RU.js
var require_ru_RU6 = __commonJS({
  "node_modules/antd/lib/locale/ru_RU.js"(exports) {
    "use strict";
    var _interopRequireDefault = require_interopRequireDefault().default;
    Object.defineProperty(exports, "__esModule", {
      value: true
    });
    exports.default = void 0;
    var _ru_RU = _interopRequireDefault(require_ru_RU());
    var _ru_RU2 = _interopRequireDefault(require_ru_RU5());
    var _ru_RU3 = _interopRequireDefault(require_ru_RU4());
    var _ru_RU4 = _interopRequireDefault(require_ru_RU3());
    var typeTemplate = "${label} не является типом ${type}";
    var localeValues = {
      locale: "ru",
      Pagination: _ru_RU.default,
      DatePicker: _ru_RU3.default,
      TimePicker: _ru_RU4.default,
      Calendar: _ru_RU2.default,
      global: {
        placeholder: "Пожалуйста выберите",
        close: "Закрыть"
      },
      Table: {
        filterTitle: "Фильтр",
        filterConfirm: "OK",
        filterReset: "Сбросить",
        filterEmptyText: "Без фильтров",
        filterCheckAll: "Выбрать все элементы",
        filterSearchPlaceholder: "Поиск в фильтрах",
        emptyText: "Нет данных",
        selectAll: "Выбрать всё",
        selectInvert: "Инвертировать выбор",
        selectNone: "Очистить все данные",
        selectionAll: "Выбрать все данные",
        sortTitle: "Сортировка",
        expand: "Развернуть строку",
        collapse: "Свернуть строку",
        triggerDesc: "Нажмите для сортировки по убыванию",
        triggerAsc: "Нажмите для сортировки по возрастанию",
        cancelSort: "Нажмите, чтобы отменить сортировку"
      },
      Tour: {
        Next: "Далее",
        Previous: "Назад",
        Finish: "Завершить"
      },
      Modal: {
        okText: "OK",
        cancelText: "Отмена",
        justOkText: "OK"
      },
      Popconfirm: {
        okText: "OK",
        cancelText: "Отмена"
      },
      Transfer: {
        titles: ["", ""],
        searchPlaceholder: "Поиск",
        itemUnit: "элем.",
        itemsUnit: "элем.",
        remove: "Удалить",
        selectAll: "Выбрать все данные",
        deselectAll: "Очистить все данные",
        selectCurrent: "Выбрать текущую страницу",
        selectInvert: "Инвертировать выбор",
        removeAll: "Удалить все данные",
        removeCurrent: "Удалить текущую страницу"
      },
      Upload: {
        uploading: "Загрузка...",
        removeFile: "Удалить файл",
        uploadError: "При загрузке произошла ошибка",
        previewFile: "Предпросмотр файла",
        downloadFile: "Загрузить файл"
      },
      Empty: {
        description: "Нет данных"
      },
      Icon: {
        icon: "иконка"
      },
      Text: {
        edit: "Редактировать",
        copy: "Копировать",
        copied: "Скопировано",
        expand: "Раскрыть",
        collapse: "Свернуть"
      },
      Form: {
        optional: "(необязательно)",
        defaultValidateMessages: {
          default: "Ошибка проверки поля ${label}",
          required: "Пожалуйста, введите ${label}",
          enum: "${label} должен быть одним из [${enum}]",
          whitespace: "${label} не может быть пустым",
          date: {
            format: "${label} не правильный формат даты",
            parse: "${label} не может быть преобразовано в дату",
            invalid: "${label} не является корректной датой"
          },
          types: {
            string: typeTemplate,
            method: typeTemplate,
            array: typeTemplate,
            object: typeTemplate,
            number: typeTemplate,
            date: typeTemplate,
            boolean: typeTemplate,
            integer: typeTemplate,
            float: typeTemplate,
            regexp: typeTemplate,
            email: typeTemplate,
            url: typeTemplate,
            hex: typeTemplate
          },
          string: {
            len: "${label} должна быть ${len} символов",
            min: "${label} должна быть больше или равна ${min} символов",
            max: "${label} должна быть меньше или равна ${max} символов",
            range: "Длина ${label} должна быть между ${min}-${max} символами"
          },
          number: {
            len: "${label} должна быть равна ${len}",
            min: "${label} должна быть больше или равна ${min}",
            max: "${label} должна быть меньше или равна ${max}",
            range: "${label} должна быть между ${min}-${max}"
          },
          array: {
            len: "Количество элементов ${label} должно быть равно ${len}",
            min: "Количество элементов ${label} должно быть больше или равно ${min}",
            max: "Количество элементов ${label} должно быть меньше или равно ${max}",
            range: "Количество элементов ${label} должно быть между ${min} и ${max}"
          },
          pattern: {
            mismatch: "${label} не соответствует шаблону ${pattern}"
          }
        }
      },
      Image: {
        preview: "Предпросмотр"
      },
      QRCode: {
        expired: "QR-код устарел",
        refresh: "Обновить"
      },
      ColorPicker: {
        presetEmpty: "Пустой",
        transparent: "Прозрачный",
        singleColor: "Один цвет",
        gradientColor: "Градиент"
      }
    };
    var _default = exports.default = localeValues;
  }
});

// node_modules/antd/locale/ru_RU.js
var require_ru_RU7 = __commonJS({
  "node_modules/antd/locale/ru_RU.js"(exports, module) {
    module.exports = require_ru_RU6();
  }
});
export default require_ru_RU7();
//# sourceMappingURL=antd_locale_ru_RU.js.map
